using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.MessageOperate.Processor.User;

class GetFriendChatListResponseProcessor(IServiceProvider container)
    : ProcessorBase<GetFriendChatListResponse>(container)
{
}