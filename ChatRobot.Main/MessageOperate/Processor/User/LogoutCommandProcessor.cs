using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.User;

public class LogoutCommandProcessor(IServiceProvider container)
    : ProcessorBase<LogoutCommand>(container)
{
    protected override async Task OnProcess(LogoutCommand message)
    {
        _userManager.OnLogoutCommand(message);
    }
}