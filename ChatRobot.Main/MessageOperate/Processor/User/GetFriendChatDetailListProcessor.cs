using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.User;

class GetGroupChatDetailListResponseProcessor(IServiceProvider container)
    : ProcessorBase<GetFriendChatDetailListResponse>(container)
{
}