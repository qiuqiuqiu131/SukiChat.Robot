using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.User;

public class UserMessageProcessor(IServiceProvider container)
    : ProcessorBase<UserMessage>(container);