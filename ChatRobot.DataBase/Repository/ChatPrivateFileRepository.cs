using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class ChatPrivateFileRepository : Repository<ChatPrivateFile>
{
    public ChatPrivateFileRepository(ChatClientDbContext dbContext) : base(dbContext)
    {
    }
}