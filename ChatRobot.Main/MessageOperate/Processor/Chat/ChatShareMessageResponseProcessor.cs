using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.Chat;

public class ChatShareMessageResponseProcessor(IServiceProvider container)
    : ProcessorBase<ChatShareMessageResponse>(container);