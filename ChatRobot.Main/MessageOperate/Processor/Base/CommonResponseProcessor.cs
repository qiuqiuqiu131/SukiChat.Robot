using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.Base;

public class CommonResponseProcessor(IServiceProvider container) 
    : ProcessorBase<CommonResponse>(container)
{
}