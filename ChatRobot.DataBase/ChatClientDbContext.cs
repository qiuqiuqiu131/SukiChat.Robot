using ChatRobot.DataBase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChatRobot.DataBase;

public class ChatClientDbContext : DbContext
{
    public DbSet<LoginHistory> LoginHistory { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<ChatPrivate> ChatPrivate { get; set; }
    public DbSet<FriendRelation> FriendRelation { get; set; }
    public DbSet<FriendReceived> FriendReceived { get; set; }

    private readonly string _databasePath;

    public ChatClientDbContext(IConfigurationRoot configurationRoot) : base()
    {
        _databasePath = configurationRoot["DataBasePath"]!;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var fileInfo = new FileInfo(_databasePath);
        if(fileInfo.Directory != null)
            Directory.CreateDirectory(fileInfo.Directory.FullName);
        optionsBuilder.UseSqlite($"Data Source={_databasePath}");
    }
}