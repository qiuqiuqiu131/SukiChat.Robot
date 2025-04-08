using AutoMapper;
using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;
using ChatServer.Common.Protobuf;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.Service;

public interface IUserLoginService
{
    Task OperateOutlineMessage(string userId, OutlineMessageResponse response);
}

internal class UserLoginService(IServiceProvider containerProvider, IMapper mapper)
    : BaseService(containerProvider), IUserLoginService
{
    public async Task OperateOutlineMessage(string userId, OutlineMessageResponse outlineResponse)
    {
        await OperateFriendRequestMesssages(userId, outlineResponse.FriendRequests);

        await OperateFriendChatMessages(userId, outlineResponse.FriendChats);
    }

    #region OperateOutLineData(处理离线未处理的消息,直接对本地数据库进行操作)

    /// <summary>
    /// 处理好友请求消息，离线时：
    /// 1.存在用户给自己发送了好友请求
    /// 2.存在用户处理了自己的好友请求
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="friendRequestMessages"></param>
    private async Task OperateFriendRequestMesssages(string userId, IList<FriendRequestMessage> friendRequestMessages)
    {
        if (friendRequestMessages.Count == 0) return;
        
        IEnumerable<FriendRequestMessage> receives =
            friendRequestMessages.Where(d => d.UserTargetId.Equals(userId));

        using (var scope = _scopedProvider.ServiceProvider.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var receiveRespository = unitOfWork.GetRepository<FriendReceived>();
            foreach (var receive in receives)
            {
                var friendReceived = mapper.Map<FriendReceived>(receive);
                var req = (FriendReceived?)await receiveRespository.GetFirstOrDefaultAsync(predicate: d =>
                    d.RequestId.Equals(friendReceived.RequestId));
                if (req != null)
                    friendReceived.Id = req.Id;
                receiveRespository.Update(friendReceived);
            }

            await unitOfWork.SaveChangesAsync();
        }
        
        friendRequestMessages.Clear();
    }
    


    /// <summary>
    /// 处理好友聊天消息，离线时可能有用户给自己发送了聊天消息
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="friendChatMessages"></param>
    private async Task OperateFriendChatMessages(string userId, IList<FriendChatMessage> friendChatMessages)
    {
        if (friendChatMessages.Count == 0) return;

        using (var scope = _scopedProvider.ServiceProvider.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var chatPrivateRepository = unitOfWork.GetRepository<ChatPrivate>();
            foreach (var chatMessage in friendChatMessages)
            {
                var chatPrivate = mapper.Map<ChatPrivate>(chatMessage);
                var result =
                    await chatPrivateRepository.GetFirstOrDefaultAsync(predicate: d => d.ChatId.Equals(chatPrivate.ChatId));
                if (result != null)
                    chatPrivate.Id = result.Id;
                chatPrivateRepository.Update(chatPrivate);
            }

            await unitOfWork.SaveChangesAsync();
        }

        friendChatMessages.Clear();
    }
    #endregion
}