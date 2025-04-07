using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.Relation;

public class UpdateGroupRelationProcessor(IServiceProvider container)
    : ProcessorBase<UpdateGroupRelation>(container);