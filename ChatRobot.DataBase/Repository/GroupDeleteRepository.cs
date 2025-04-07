using ChatRobot.DataBase.Data;
using ChatRobot.DataBase.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ChatRobot.DataBase.Repository;

public class GroupDeleteRepository(DbContext dbContext)
    : Repository<GroupDelete>(dbContext);