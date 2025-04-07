using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class GroupRepository : Repository<Group>
{
    public GroupRepository(ChatClientDbContext dbContext) : base(dbContext)
    {
    }
}