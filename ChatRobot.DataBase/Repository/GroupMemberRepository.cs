using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ChatRobot.DataBase.Repository;

public class GroupMemberRepository(DbContext dbContext)
    : Repository<GroupMember>(dbContext);