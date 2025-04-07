using ChatRobot.Client.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ChatRobot.Client
{
    public abstract class Application
    {
        /// <summary>
        /// IOC容器
        /// </summary>
        private IServiceProvider services;

        protected IServiceProvider Services => services;

        /// <summary>
        /// 配置
        /// </summary>
        private IConfigurationRoot configuration;

        protected IConfigurationRoot Configuration => configuration;

        protected ISocketClient SocketClient => services.GetService<ISocketClient>()!;

        public Application()
        { 
            // 配置初始化
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            InitConfigurations(configurationBuilder);
            configuration = configurationBuilder.Build();

            // 日志初始化
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // IOC容器初始化
            IServiceCollection builder = new ServiceCollection();
            RegisterServices(builder);
            services = builder.BuildServiceProvider();
        }

        /// <summary>
        /// IOC容器注册服务
        /// </summary>
        /// <param name="registrator"></param>
        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(configuration);
            services.AddSingleton(Log.Logger);
            services.AddSingleton<ISocketClient,SocketClient>();
            RegisterServicesExtens(services);
        }

        /// <summary>
        /// 程序服务初始化
        /// </summary>
        /// <param name="registrator"></param>
        protected virtual void RegisterServicesExtens(IServiceCollection services) { }
    
        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="configurationBuilder"></param>
        protected virtual void InitConfigurations(IConfigurationBuilder configurationBuilder)
        { 
            configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        }
        
        /// <summary>
        /// 程序初始化完成
        /// </summary>
        /// <param name="serviceProvider"></param>
        protected virtual void OnInitialized(IServiceProvider serviceProvider) { }

        /// <summary>
        /// 添加Socket处理程序
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void ChannelHandler(SocketClientBuilder builder);

        /// <summary>
        /// 启动服务器
        /// </summary>
        public void Start()
        {
            OnInitialized(services);
            
            // 创建Socket服务,添加处理程序
            SocketClientBuilder builder = new SocketClientBuilder();
            ChannelHandler(builder);
           
            // 配置ChannelHandler
            SocketClient.Init(builder);

            SocketClient.ConnectedEvent += OnConnected;
            SocketClient.DisconnectedEvent += OnDisconnected;
            // 启动服务器
            SocketClient.Start();
        }

        protected virtual void OnConnected() { }
        
        protected virtual void OnDisconnected() { }
    
        /// <summary>
        /// 关闭服务器
        /// </summary>
        public async Task Close() => await SocketClient.Stop();
    }
}
