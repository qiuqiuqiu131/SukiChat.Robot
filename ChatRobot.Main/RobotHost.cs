using AutoMapper;
using ChatRobot.Client;
using ChatRobot.Client.Client;
using ChatRobot.DataBase;
using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;
using ChatRobot.Main.Entity;
using ChatRobot.Main.Helper;
using ChatRobot.Main.IOServer.Handler;
using ChatRobot.Main.Manager;
using ChatRobot.Main.Mapper;
using ChatRobot.Main.MessageOperate;
using ChatRobot.Main.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ChatRobot.Main;

public class RobotHost : Application
{
    public Robot Robot { get;}
    
    public RobotHost(Robot robot)
    {
        Robot = robot;
    }
    
    /// <summary>
    /// 配置Dotnetty管道
    /// </summary>
    /// <param name="builder"></param>
    protected override void ChannelHandler(SocketClientBuilder builder)
    {
        builder.AddHandler<ClientConnectHandler>();
        builder.AddHandler<EchoClientHandler>();
    }

    protected override void InitConfigurations(IConfigurationBuilder configurationBuilder)
    {
        base.InitConfigurations(configurationBuilder);
        configurationBuilder.AddJsonFile("usersettings.json", optional: true, reloadOnChange: true);
    }

    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="services"></param>
    protected override void RegisterServicesExtens(IServiceCollection services)
    {
        // 注册Mapper
        var mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProtoToDataProfile>();
        }).CreateMapper();
        services.AddSingleton(mapConfig);

        // 注册事件聚合器
        services.AddSingleton<IEventAggregator, EventAggregator>();
        
        // 添加处理器
        services.AddProcessors();
        
        // 添加ChannelHandler
        services.AddTransient<ClientConnectHandler>();
        services.AddTransient<EchoClientHandler>();

        // 添加数据库
        services.RegisterDataBase();
        
        // 添加SocketClient
        services.AddSingleton<ProtobufDispatcher>();
        services.AddSingleton<IProtobufDispatcher>(d => d.GetRequiredService<ProtobufDispatcher>());
        services.AddSingleton<IServer>(d => d.GetRequiredService<ProtobufDispatcher>());
        
        // 添加UserManager
        services.AddSingleton<IUserManager, UserManager>();

        // 添加Helper
        services.AddTransient<IAIChatHelper, AIChatHelper>();
        services.AddTransient<IMessageHelper, MessageHelper>();
        
        // 添加Service
        services.AddTransient<IChatRemoteService, ChatRemoteService>();
        services.AddTransient<ILoginService, LoginService>();
        services.AddTransient<IUserLoginService, UserLoginService>();
        services.AddTransient<IChatService, ChatService>();
    }

    /// <summary>
    /// 程序初始化完成时调用
    /// </summary>
    /// <param name="serviceProvider"></param>
    protected override void OnInitialized(IServiceProvider serviceProvider)
    {
        // 启动服务
        var server = serviceProvider.GetRequiredService<IServer>();
        server.Start();
        
        var logger = serviceProvider.GetRequiredService<ILogger>();
        logger.Information("机器人服务已启动");
    }

    /// <summary>
    /// 连接时调用
    /// </summary>
    protected override async void OnConnected()
    {
        var logger = Services.GetRequiredService<ILogger>();
        logger.Information("成功连接服务器");
        
        // 尝试登录
        var userManager = Services.GetRequiredService<IUserManager>();
        userManager.Robot = Robot;
        await userManager.Login(Robot.User.ID!, Robot.User.Password!);
    }
    
    /// <summary>
    /// 断开连接时调用
    /// </summary>
    protected override void OnDisconnected()
    {
        var logger = Services.GetRequiredService<ILogger>();
        logger.Information("与服务器断开连接");
        
        // 注销登录
        var userManager = Services.GetRequiredService<IUserManager>();
        userManager.OnLogout();
    }
}