using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class ChatGroupFileRepository : Repository<ChatGroupFile>
{
    public ChatGroupFileRepository(ChatClientDbContext dbContext) : base(dbContext)
    {
    }
}