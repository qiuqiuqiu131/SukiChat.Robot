using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.FriendRelation;

public class FriendResponeseProcessor(IServiceProvider container)
    : ProcessorBase<FriendResponseFromServer>(container);