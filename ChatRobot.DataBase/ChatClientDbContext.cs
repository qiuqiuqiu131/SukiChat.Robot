using ChatRobot.DataBase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChatRobot.DataBase;

public class ChatClientDbContext : DbContext
{
    public DbSet<LoginHistory> LoginHistory { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<ChatPrivate> ChatPrivate { get; set; }
    public DbSet<ChatPrivateDetail> ChatPrivateDetails { get; set; }
    public DbSet<ChatPrivateFile> ChatPrivateFiles { get; set; }
    public DbSet<FriendRelation> FriendRelation { get; set; }
    public DbSet<FriendRequest> FriendRequest { get; set; }
    public DbSet<FriendReceived> FriendReceived { get; set; }
    public DbSet<FriendDelete> FriendDelete { get; set; }

    public DbSet<Group> Group { get; set; }
    public DbSet<GroupRequest> GroupRequest { get; set; }
    public DbSet<GroupReceived> GroupReceiveds { get; set; }
    public DbSet<GroupRelation> GroupRelation { get; set; }
    public DbSet<ChatGroup> ChatGroup { get; set; }
    public DbSet<ChatGroupDetail> ChatGroupDetails { get; set; }
    public DbSet<ChatGroupFile> ChatGroupFiles { get; set; }
    public DbSet<GroupMember> GroupMember { get; set; }
    public DbSet<GroupDelete> GroupDelete { get; set; }

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