using ChatRobot.Client.Client;
using ChatServer.Common;
using ChatServer.Common.Protobuf;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.MessageOperate.Processor.Base;

public class HeartBeatProcessor(IServiceProvider container)
    : ProcessorBase<HeartBeat>(container)
{
    protected override async Task OnProcess(HeartBeat message)
    {
        var _client = _container.GetRequiredService<ISocketClient>();
        if (_client.Channel != null && _client.Channel.Active)
        {
            await _client.Channel.WriteAndFlushProtobufAsync(message);
        }
    }
}