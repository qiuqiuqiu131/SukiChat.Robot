﻿using System.Net;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ChatRobot.Client.Client;

public class SocketClient : ISocketClient
    {
        private readonly IConfigurationRoot configuration;
        private readonly IServiceProvider services;

        private MultithreadEventLoopGroup? group;
        private Bootstrap? bootstrap;

        public IChannel? Channel { get; private set; }

        private EndPoint endPoint;
        private readonly (int, int, int) reconnectConfig;

        private List<Type>? channels;

        private int reconnectCount = 0;

        public SocketClient(IServiceProvider serviceProvider)
        {
            this.services = serviceProvider;
            this.configuration = services.GetService<IConfigurationRoot>()!;

            endPoint = GetAddress();
            reconnectConfig = GetReconnect();
        }

        public event Action ConnectedEvent;
        public event Action DisconnectedEvent;
        
        /// <summary>
        /// 配置ChannelHandler
        /// </summary>
        /// <param name="builder"></param>
        public void Init(SocketClientBuilder builder)
        {
            channels = builder.GetChannels();
        }

        public async void Start() => ClientConnectAsync();

        public async Task Stop()
        {
            if (Channel is { Active: true })
                await Channel.CloseAsync();
            Channel = null;
        }

        /// <summary>
        /// 服务器启动
        /// </summary>
        protected virtual async Task ClientConnectAsync()
        {
            // 设置环境变量,不记录已发字节流
            Environment.SetEnvironmentVariable("io.netty.allocator.numDirectArenas", "0");
            Environment.SetEnvironmentVariable("io.netty.allocator.numHeapArenas", "0");

            group ??= new MultithreadEventLoopGroup();

            if (bootstrap == null)
            {
                bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.ConnectTimeout, TimeSpan.FromSeconds(reconnectConfig.Item1))
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                        pipeline.AddLast(new IdleStateHandler(0, 0, reconnectConfig.Item3));

                        if (channels == null) return;
                        foreach (var type in channels)
                        {
                            var handle = (IChannelHandler)services.GetService(type)!;
                            pipeline.AddLast(handle);
                        }
                    }));
                bootstrap.RemoteAddress(endPoint);
            }

            // 尝试连接服务器
            try
            {
                IChannel channel = await bootstrap!.ConnectAsync();
                if (channel.Open || channel.Active)
                {
                    Channel = channel;
                    ConnectedEvent?.Invoke();   
                    _ = Channel.CloseCompletion.ContinueWith((t, s) =>
                        {
                            DisconnectedEvent?.Invoke();
                            scheduleReconnect();
                        }, this,
                        TaskContinuationOptions.ExecuteSynchronously);
                }
                else
                {
                    scheduleReconnect();
                }
            }
            catch (Exception ex)
            {
                scheduleReconnect();
            }
        }

        private async void scheduleReconnect()
        {
            reconnectCount++;
            if (group != null)
                group.Schedule(() => ClientConnectAsync(), TimeSpan.FromSeconds(reconnectConfig.Item2));
        }

        #region Congiuration

        /// <summary>
        /// 获取配置服务器地址
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private IPEndPoint GetAddress()
        {
            string? ip = configuration["Address:IP"];
            if (ip == null)
                throw new ArgumentNullException($"Configuration didn't exits Address:IP");
            string? port = configuration["Address:Main_Port"];
            if (port == null)
                throw new ArgumentNullException($"Configuration didn't exits Address:Port");

            return new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
        }

        /// <summary>
        /// 获取重连配置
        /// </summary>
        /// <returns></returns>
        private (int, int, int) GetReconnect()
        {
            int connectTime = configuration["Reconnect:ConnectTime"] == null
                ? 3
                : int.Parse(configuration["Reconnect:ConnectTime"]!);
            int reconnectTime = configuration["Reconnect:ReconnectTime"] == null
                ? 5
                : int.Parse(configuration["Reconnect:ReconnectTime"]!);
            int allIdleTime = configuration["Reconnect:AllIdleTime"] == null
                ? 10
                : int.Parse(configuration["Reconnect:AllIdleTime"]!);
            return (connectTime, reconnectTime, allIdleTime);
        }

        #endregion
    }