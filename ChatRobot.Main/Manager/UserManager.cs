using ChatRobot.Client.Client;
using ChatServer.Common;
using ChatServer.Common.Protobuf;
using Serilog;

namespace ChatRobot.Main.Manager;

public interface IUserManager
{
    bool IsLogin { get; }
    string UserId { get; }
    
    Task<bool> Login(string userId, string password);
    void OnLoginResponse(LoginResponse response);

    void OnLogoutCommand(LogoutCommand message);
    void OnLogout();
}

public class UserManager : IUserManager
{
    private readonly ISocketClient _socketClient;
    private readonly ILogger _logger;

    public bool IsLogin { get; private set; }
    public string UserId { get; private set; }

    public UserManager(ISocketClient socketClient,ILogger logger)
    {
        _socketClient = socketClient;
        _logger = logger;
    }

    #region Login

    private TaskCompletionSource<bool>? _taskCompletionSource;
    public async Task<bool> Login(string userId, string password)
    {
        _taskCompletionSource = new TaskCompletionSource<bool>();
        var loginRequest = new LoginRequest
        {
            Id = userId,
            Password = password,
        };
        await _socketClient.Channel!.WriteAndFlushProtobufAsync(loginRequest);
        // 等待登录结果
        var result = await _taskCompletionSource.Task;
        if(result)
        {
            IsLogin = true;
            UserId = userId;
            _logger.Information("机器人\"{UserId}\" 登录成功", userId);
        }
        else
        {
            IsLogin = false;
            UserId = string.Empty;
            _logger.Error("登录失败,请检查用户名和密码");
        }
        
        _taskCompletionSource = null;
        return result;
    }
    
    public void OnLoginResponse(LoginResponse response)
    {
        if (_taskCompletionSource == null || _taskCompletionSource.Task.IsCompleted) return;
        if (response is { Response: { State: true } })
            _taskCompletionSource.SetResult(true);
        else
            _taskCompletionSource.SetResult(false);
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