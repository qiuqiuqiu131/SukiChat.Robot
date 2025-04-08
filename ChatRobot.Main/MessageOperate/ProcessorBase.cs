using ChatRobot.Client.Client;
using ChatRobot.Main.Event;
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
    private readonly IEventAggregator _eventAggregator;

    public ProcessorBase(IServiceProvider container)
    {
        _container = container;
        _userManager = container.GetRequiredService<IUserManager>();
        _eventAggregator = container.GetRequiredService<IEventAggregator>();
    }

    /// <summary>
    /// 未拦截的消息处理
    /// </summary>
    /// <param name="message"></param>
    public virtual async Task Process(T message)
    {
        // 发送消息事件
        _eventAggregator.GetEvent<ResponseEvent<T>>().Publish(message);
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
}