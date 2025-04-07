using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.User;

public class GetUserMessageResponse(IServiceProvider container)
    : ProcessorBase<GetUserResponse>(container);