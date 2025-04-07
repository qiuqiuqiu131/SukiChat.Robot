using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.FriendRelation;

public class FriendResponseFromClientResponseProcessor(IServiceProvider container)
    : ProcessorBase<FriendResponseFromClientResponse>(container)
{
}