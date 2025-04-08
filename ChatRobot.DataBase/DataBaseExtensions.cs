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
        containerRegistry.AddCustomRepository<FriendRelation, FriendRelationRepository>();
    }
}