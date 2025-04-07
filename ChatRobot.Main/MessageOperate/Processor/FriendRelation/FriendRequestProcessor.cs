using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.FriendRelation;

public class FriendRequestProcessor(IServiceProvider container) : ProcessorBase<FriendRequestFromServer>(container);