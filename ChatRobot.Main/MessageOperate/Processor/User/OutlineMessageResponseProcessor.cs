using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.User;

public class OutlineMessageResponseProcessor(IServiceProvider container)
    : ProcessorBase<OutlineMessageResponse>(container);