using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class GroupRequestRepository : Repository<GroupRequest>
{
    public GroupRequestRepository(ChatClientDbContext dbContext) : base(dbContext)
    {
    }
}