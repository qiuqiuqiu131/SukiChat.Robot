using ChatRobot.Client;
using ChatRobot.Client.Client;
using ChatRobot.DataBase;
using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;
using ChatRobot.Main.Helper;
using ChatRobot.Main.IOServer.Handler;
using ChatRobot.Main.Manager;
using ChatRobot.Main.MessageOperate;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ChatRobot.Main;

public class RobotHost : Application
{
    protected override void ChannelHandler(SocketClientBuilder builder)
    {
        builder.AddHandler<ClientConnectHandler>();
        builder.AddHandler<EchoClientHandler>();
    }

    protected override void RegisterServicesExtens(IServiceCollection services)
    {
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
    }

    protected override void OnInitialized(IServiceProvider serviceProvider)
    {
        // 启动服务
        var server = serviceProvider.GetRequiredService<IServer>();
        server.Start();
        
        var logger = serviceProvider.GetRequiredService<ILogger>();
        logger.Information("机器人服务已启动");
    }

    protected override async void OnConnected()
    {
        var logger = Services.GetRequiredService<ILogger>();
        logger.Information("成功连接服务器");

        var configuration = Services.GetRequiredService<IConfigurationRoot>();
        var userId = configuration["User:ID"];
        var password = configuration["User:Password"];
        
        // 尝试登录
        var userManager = Services.GetRequiredService<IUserManager>();
        await userManager.Login(userId!, password!);
    }
    
    protected override void OnDisconnected()
    {
        var logger = Services.GetRequiredService<ILogger>();
        logger.Information("与服务器断开连接");
        
        // 注销登录
        var userManager = Services.GetRequiredService<IUserManager>();
        userManager.OnLogout();
    }
}