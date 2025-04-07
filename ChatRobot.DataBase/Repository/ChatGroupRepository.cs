using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class ChatGroupRepository : Repository<ChatGroup>
{
    public ChatGroupRepository(ChatClientDbContext dbContext) : base(dbContext)
    {
    }
}