using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class FriendReceivedRepository : Repository<FriendReceived>
{
    public FriendReceivedRepository(ChatClientDbContext dbContext) : base(dbContext)
    {
    }
}