using AutoMapper;
using ChatRobot.DataBase.Data;
using ChatRobot.Main.Tool;
using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.Mapper;

public class ProtoToDataProfile : Profile
{
    public ProtoToDataProfile()
    {
        #region FriendChatMessage + ChatPrivate
        
        CreateMap<FriendChatMessage, ChatPrivate>()
            .ForMember(cp => cp.Message, opt => opt.MapFrom(cm => ChatMessageTool.EncruptChatMessage(cm.Messages)))
            .ForMember(cp => cp.Time, opt => opt.MapFrom(cm => DateTime.Parse(cm.Time)))
            .ForMember(cp => cp.ChatId, opt => opt.MapFrom(cm => cm.Id))
            .ForMember(cp => cp.Id, opt => opt.Ignore())
            .ForMember(cp => cp.RetractedTime,
                opt => opt.MapFrom(cm =>
                    string.IsNullOrWhiteSpace(cm.RetractTime) ? DateTime.MinValue : DateTime.Parse(cm.RetractTime)));
        
        CreateMap<ChatPrivate, FriendChatMessage>()
            .ForMember(cm => cm.Messages, opt => opt.MapFrom(cp => ChatMessageTool.DecruptChatMessage(cp.Message)))
            .ForMember(cm => cm.Time, opt => opt.MapFrom(cp => cp.Time.ToString()))
            .ForMember(cm => cm.Id, opt => opt.MapFrom(cp => cp.ChatId))
            .ForMember(cm => cm.RetractTime, opt => opt.MapFrom(cp => cp.RetractedTime.ToString()));
        
        #endregion

        #region NewFriendMessage + FriendRelation

        CreateMap<NewFriendMessage, FriendRelation>()
            .ForMember(fr => fr.GroupTime, opt => opt.MapFrom(nf => DateTime.Parse(nf.RelationTime)))
            .ForMember(fr => fr.User1Id, opt => opt.MapFrom(nf => nf.UserId))
            .ForMember(fr => fr.User2Id, opt => opt.MapFrom(nf => nf.FrinedId))
            .ForMember(fr => fr.Remark, opt => opt.MapFrom(nf => string.IsNullOrEmpty(nf.Remark) ? null : nf.Remark));
        
        #endregion

        #region FriendRequestMessage

        // User login state, get outline message and operate friend request
        CreateMap<FriendRequestMessage, FriendReceived>()
            .ForMember(fr => fr.ReceiveTime, opt => opt.MapFrom(fm => DateTime.Parse(fm.RequestTime)))
            .ForMember(fr => fr.SolveTime, opt =>
                opt.MapFrom(fm =>
                    string.IsNullOrEmpty(fm.SolvedTime) ? (DateTime?)null : DateTime.Parse(fm.SolvedTime)));
        #endregion

        #region UserDetailMessage + User

        CreateMap<UserDetailMessage, User>()
            .ForMember(u => u.LastReadFriendMessageTime,
                opt => opt.MapFrom(um => DateTime.Parse(um.LastReadFriendMessageTime)))
            .ForMember(u => u.LastReadGroupMessageTime,
                opt => opt.MapFrom(um => DateTime.Parse(um.LastReadGroupMessageTime)))
            .ForMember(u => u.LastDeleteFriendMessageTime,
                opt => opt.MapFrom(um => DateTime.Parse(um.LastDeleteFriendMessageTime)))
            .ForMember(u => u.LastDeleteGroupMessageTime,
                opt => opt.MapFrom(um => DateTime.Parse(um.LastDeleteGroupMessageTime)))
            .ForMember(u => u.RegisteTime, opt => opt.MapFrom(um => DateTime.Parse(um.RegisterTime)))
            .ForMember(u => u.Birthday, opt => opt.MapFrom(um => string.IsNullOrEmpty(um.Birth) ? (DateOnly?)null : DateOnly.Parse(um.Birth)))
            .ForMember(u => u.EmailNumber,
                opt => opt.MapFrom(um => string.IsNullOrEmpty(um.EmailNumber) ? null : um.EmailNumber))
            .ForMember(u => u.PhoneNumber,
                opt => opt.MapFrom(um => string.IsNullOrEmpty(um.PhoneNumber) ? null : um.PhoneNumber));

        CreateMap<User, UserDetailMessage>()
            .ForMember(u => u.LastDeleteFriendMessageTime,
                opt => opt.MapFrom(um => um.LastDeleteFriendMessageTime.ToString()))
            .ForMember(u => u.LastDeleteGroupMessageTime,
                opt => opt.MapFrom(um => um.LastDeleteGroupMessageTime.ToString()))
            .ForMember(u => u.LastReadFriendMessageTime,
                opt => opt.MapFrom(um => um.LastReadFriendMessageTime.ToString()))
            .ForMember(u => u.LastReadGroupMessageTime,
                opt => opt.MapFrom(um => um.LastReadGroupMessageTime.ToString()))
            .ForMember(u => u.RegisterTime,
                opt => opt.MapFrom(um => um.RegisteTime.ToString()))
            .ForMember(u => u.Introduction, opt => opt.MapFrom(um => um.Introduction ?? string.Empty))
            .ForMember(u => u.Birth,
                opt => opt.MapFrom(um => um.Birthday == null ? string.Empty : um.Birthday.ToString()))
            .ForMember(u => u.EmailNumber, opt => opt.MapFrom(um => um.EmailNumber ?? string.Empty))
            .ForMember(u => u.PhoneNumber, opt => opt.MapFrom(um => um.PhoneNumber ?? string.Empty));

        #endregion
    }
}