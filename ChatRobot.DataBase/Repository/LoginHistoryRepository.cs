using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class LoginHistoryRepository:Repository<LoginHistory>
{
    public LoginHistoryRepository(ChatClientDbContext dbContext) : base(dbContext)
    {
    }
}