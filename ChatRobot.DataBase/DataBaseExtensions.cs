using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.Repository;
using ChatRobot.DataBase.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.DataBase;

public static class DataBaseExtensions
{
    public static void RegisterDataBase(this IServiceCollection containerRegistry)
    {
        // 注册Sqlite
        containerRegistry.AddScoped<ChatClientDbContext>();

        // 注册工作单元
        containerRegistry.AddUnitOfWork<ChatClientDbContext>();

        // 注册仓储
        containerRegistry.AddCustomRepository<LoginHistory, LoginHistoryRepository>();
        containerRegistry.AddCustomRepository<User, UserRepository>();
        containerRegistry.AddCustomRepository<ChatPrivate, ChatPrivateRepository>();
        containerRegistry.AddCustomRepository<ChatPrivateDetail, ChatPrivateDetailRepository>();
        containerRegistry.AddCustomRepository<FriendRelation, FriendRelationRepository>();
        containerRegistry.AddCustomRepository<FriendRequest, FriendRequestRepository>();
        containerRegistry.AddCustomRepository<FriendDelete, FriendDeleteRepository>();
        containerRegistry.AddCustomRepository<ChatPrivateFile, ChatPrivateFileRepository>();

        containerRegistry.AddCustomRepository<Group, GroupRepository>();
        containerRegistry.AddCustomRepository<GroupRequest, GroupRequestRepository>();
        containerRegistry.AddCustomRepository<GroupRelation, GroupRelationRepository>();
        containerRegistry.AddCustomRepository<ChatGroup, ChatGroupRepository>();
        containerRegistry.AddCustomRepository<ChatGroupDetail, ChatGroupDetailRepository>();
        containerRegistry.AddCustomRepository<GroupDelete, GroupDeleteRepository>();
        containerRegistry.AddCustomRepository<ChatGroupFile, ChatGroupFileRepository>();
    }
}