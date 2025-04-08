using ChatRobot.Main.Manager;
using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.User;

public class LoginResponseProcessor(IServiceProvider container) : ProcessorBase<LoginResponse>(container);