using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ChatRobot.DataBase.Repository;

public class ChatPrivateRepository : Repository<ChatPrivate>
{
    public ChatPrivateRepository(DbContext dbContext) : base(dbContext){}
}