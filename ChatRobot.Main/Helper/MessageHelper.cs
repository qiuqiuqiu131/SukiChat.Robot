using ChatRobot.Client.Client;
using ChatRobot.Main.Event;
using ChatServer.Common;
using Google.Protobuf;
using Microsoft.Extensions.Configuration;

namespace ChatRobot.Main.Helper;

public interface IMessageHelper
{
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<bool> SendMessage(IMessage message);

    /// <summary>
    /// 发送消息并获取相应消息
    /// </summary>
    /// <param name="message"></param>
    /// <typeparam name="T">想要获取的相应消息</typeparam>
    /// <returns></returns>
    Task<T?> SendMessageWithResponse<T>(IMessage message) where T : IMessage;
}

internal class MessageHelper : IMessageHelper
{
    private readonly ISocketClient client;
    private readonly IEventAggregator eventAggregator;
    private readonly IConfigurationRoot configuration;

    public MessageHelper(ISocketClient client, IEventAggregator eventAggregator, IConfigurationRoot configuration)
    {
        this.client = client;
        this.eventAggregator = eventAggregator;
        this.configuration = configuration;
    }


    public async Task<bool> SendMessage(IMessage message)
    {
        if (client.Channel == null)
            return false;

        try
        {
            await client.Channel.WriteAndFlushProtobufAsync(message);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<T?> SendMessageWithResponse<T>(IMessage message) where T : IMessage
    {
        TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
        var token = eventAggregator.GetEvent<ResponseEvent<T>>().Subscribe(e =>
        {
            if (!taskCompletionSource.Task.IsCompleted)
                taskCompletionSource.SetResult(e);
        });

        if (client.Channel == null)
            return default;

        try
        {
            // 发送消息
            await client.Channel.WriteAndFlushProtobufAsync(message);

            Task wait = Task.Delay(TimeSpan.FromSeconds(int.Parse(configuration["OutOfTime"]!)));
            var task = await Task.WhenAny(taskCompletionSource.Task, wait);
            token?.Dispose();


            if (task == taskCompletionSource.Task)
            {
                return taskCompletionSource.Task.Result;
            }
            else
            {
                taskCompletionSource.SetCanceled();
                return default;
            }
        }
        catch
        {
            token?.Dispose();
            return default;
        }
    }
}