using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class FriendRelationRepository : Repository<FriendRelation>
{
    public FriendRelationRepository(ChatClientDbContext dbContext) : base(dbContext){}
}