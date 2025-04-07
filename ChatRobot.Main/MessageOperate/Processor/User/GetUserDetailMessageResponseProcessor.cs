using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.User;

public class GetUserDetailMessageResponseProcessor(IServiceProvider container)
    : ProcessorBase<GetUserDetailMessageResponse>(container);