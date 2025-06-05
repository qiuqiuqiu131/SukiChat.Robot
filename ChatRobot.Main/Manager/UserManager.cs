using AutoMapper;
using ChatRobot.Client.Client;
using ChatRobot.DataBase.Data;
using ChatRobot.Main.Entity;
using ChatRobot.Main.Helper;
using ChatRobot.Main.Service;
using ChatServer.Common;
using ChatServer.Common.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ChatRobot.Main.Manager;

public interface IUserManager
{
    Robot Robot { get; set; }
    bool IsLogin { get; }
    string UserId { get; }
    User User { get;}
    
    Task<bool> Login(string userId, string password);
    void OnLogoutCommand(LogoutCommand message);
    void OnLogout();
}

public class UserManager : IUserManager
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;

    public Robot Robot { get; set; }
    public bool IsLogin { get; private set; }
    public string UserId { get; private set; }
    public User User { get; private set; }

    public UserManager(ILogger logger,IServiceProvider serviceProvider,IMapper mapper)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    #region Login
    public async Task<bool> Login(string userId, string password)
    {
        var messageHelper = _serviceProvider.GetRequiredService<IMessageHelper>();
        
        // -- 1、登录请求 -- //
        var loginRequest = new LoginRequest
        {
            Id = userId,
            Password = password,
        };
        var result1 = await messageHelper.SendMessageWithResponse<LoginResponse>(loginRequest);
        
        // 处理登录结果
        if(result1 == null || result1.Response.State == false)
        {
            IsLogin = false;
            UserId = string.Empty;
            _logger.Error("登录失败,请检查用户名和密码");
            return false;
        }
        
        // -- 2、处理离线数据 -- //
        var loginService = _serviceProvider.GetRequiredService<ILoginService>();
        var lastLoginTime = await loginService.GetLastLoginTime(userId);
        var outlineRequest = new OutlineMessageRequest
        {
            Id = userId,
            LastLogoutTime = lastLoginTime.ToString()
        };
        var result2 = await messageHelper.SendMessageWithResponse<OutlineMessageResponse>(outlineRequest);
        
        if(result2 == null)
        {
            IsLogin = false;
            UserId = string.Empty;
            _logger.Error("登录失败,请检查用户名和密码");
            return false;
        }

        // 处理离线数据
        var userLoginService = _serviceProvider.GetRequiredService<IUserLoginService>();
        await userLoginService.OperateOutlineMessage(userId, result2);

        // -- 3、获取用户详细信息 -- //
        var userDetailRequest = new GetUserDetailMessageRequest
        {
            Id = userId,
            Password = password
        };
        var result3 = await messageHelper.SendMessageWithResponse<GetUserDetailMessageResponse>(userDetailRequest);

        if (result3 == null || result3.Response.State == false)
        {
            IsLogin = false;
            UserId = string.Empty;
            _logger.Error("登录失败，用户信息出错");
            return false;
        }

        await loginService.Login(userId, password);

        // 登录成功，初始化账号信息
        IsLogin = true;
        UserId = userId;
        User = _mapper.Map<User>(result3.User);
        _logger.Information("机器人\"{UserId}\" 登录成功", userId);

        return true;
    }

    public void OnLogoutCommand(LogoutCommand message)
    {
        OnLogout();
    }

    #endregion

    public void OnLogout()
    {
        _logger.Information("机器人\"{UserId}\" 已登出", UserId);
        
        IsLogin = false;
        UserId = string.Empty;
    }
}