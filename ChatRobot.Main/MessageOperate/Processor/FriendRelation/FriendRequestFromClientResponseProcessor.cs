using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.FriendRelation;

public class FriendRequestFromClientResponseProcessor(IServiceProvider containerProvider)
    : ProcessorBase<FriendRequestFromClientResponse>(containerProvider);