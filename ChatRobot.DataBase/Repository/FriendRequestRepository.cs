using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class FriendRequestRepository : Repository<FriendRequest>
{
    public FriendRequestRepository(ChatClientDbContext dbContext) : base(dbContext)
    {
    }
}