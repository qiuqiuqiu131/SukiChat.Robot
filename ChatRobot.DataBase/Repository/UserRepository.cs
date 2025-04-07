using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class UserRepository:Repository<User>
{
    public UserRepository(ChatClientDbContext dbContext) : base(dbContext)
    {
    }
}