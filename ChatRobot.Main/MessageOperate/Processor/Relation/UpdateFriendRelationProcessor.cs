using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.Relation;

public class UpdateFriendRelationProcessor(IServiceProvider container)
    : ProcessorBase<UpdateFriendRelation>(container);