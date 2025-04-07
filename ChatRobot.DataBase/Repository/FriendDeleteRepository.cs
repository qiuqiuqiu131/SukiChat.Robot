using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;

namespace ChatRobot.DataBase.Repository;

public class FriendDeleteRepository(ChatClientDbContext dbContext)
    : Repository<FriendDelete>(dbContext);