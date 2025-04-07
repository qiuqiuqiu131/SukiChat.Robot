using ChatRobot.Client.Client;
using ChatRobot.Main.Manager;
using ChatServer.Common;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.MessageOperate;

public class ProcessorBase<T> : IProcessor<T>
    where T : IMessage
{
    protected readonly IServiceProvider _container;
    protected readonly IUserManager _userManager;
    protected readonly ISocketClient _socketClient;

    public ProcessorBase(IServiceProvider container)
    {
        _container = container;
        _userManager = container.GetRequiredService<IUserManager>();
        _socketClient = container.GetRequiredService<ISocketClient>();
    }

    /// <summary>
    /// 未拦截的消息处理
    /// </summary>
    /// <param name="message"></param>
    public virtual async Task Process(T message)
    {
        if(_userManager.IsLogin)
            await OnProcess(message);
    }

    /// <summary>
    /// 拦截后处理消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected virtual Task OnProcess(T message)
    {
        return Task.CompletedTask;
    }

    protected async Task SendMessage(IMessage message)
    {
        if(_socketClient.Channel is { Open: true })
        {
            await _socketClient.Channel.WriteAndFlushProtobufAsync(message);
        }
    }
}