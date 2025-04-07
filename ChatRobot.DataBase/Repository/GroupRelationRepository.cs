using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class GroupRelationRepository : Repository<GroupRelation>
{
    public GroupRelationRepository(ChatClientDbContext dbContext) : base(dbContext)
    {
    }
}