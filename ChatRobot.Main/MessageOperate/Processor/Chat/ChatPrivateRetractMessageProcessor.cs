using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.Chat;

public class ChatPrivateRetractMessageProcessor(IServiceProvider container)
    : ProcessorBase<ChatPrivateRetractMessage>(container);