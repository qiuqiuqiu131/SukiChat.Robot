using AutoMapper;
using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;
using ChatServer.Common.Protobuf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.Service;

public interface IChatService
{
    Task<List<FriendChatMessage>> GetFriendChatMessages(string userId, string friendId,int count = 15);
    
    Task AddFriendChatMessage(string userId, FriendChatMessage friendChatMessage);
}

public class ChatService:BaseService, IChatService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public ChatService(IServiceProvider containerProvider,IMapper mapper) : base(containerProvider)
    {
        _mapper = mapper;
        _unitOfWork = containerProvider.GetRequiredService<IUnitOfWork>();
    }

    public async Task<List<FriendChatMessage>> GetFriendChatMessages(string userId, string friendId,int count = 15)
    {
        var chatPrivateRepository = _unitOfWork.GetRepository<ChatPrivate>();
        var query = chatPrivateRepository.GetAll(
            predicate:d => (d.UserFromId.Equals(userId) && d.UserTargetId.Equals(friendId)) 
                           || (d.UserFromId.Equals(friendId) && d.UserTargetId.Equals(userId)),
            orderBy:d => d.OrderByDescending(c => c.ChatId)).Take(count).Reverse();
        var chatMessages = await query.ToListAsync();
        return _mapper.Map<List<FriendChatMessage>>(chatMessages);
    }

    public async Task AddFriendChatMessage(string userId, FriendChatMessage friendChatMessage)
    {
        try
        {
            var chatPrivateRepository = _unitOfWork.GetRepository<ChatPrivate>();
            var chatPrivate = _mapper.Map<ChatPrivate>(friendChatMessage);
            var entity = await chatPrivateRepository.GetFirstOrDefaultAsync(predicate: d => d.ChatId.Equals(chatPrivate.ChatId));
            if (entity != null)
                chatPrivate.Id = entity.Id;
            chatPrivateRepository.Update(chatPrivate);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // doNothing
        }
    }
}