using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.Chat;

public class ChatPrivateDeleteResponseProcessor(IServiceProvider container)
    : ProcessorBase<ChatPrivateDeleteResponse>(container);