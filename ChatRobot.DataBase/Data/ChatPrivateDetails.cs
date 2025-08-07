using Microsoft.EntityFrameworkCore;

namespace ChatRobot.DataBase.Data;

[PrimaryKey(nameof(UserId), nameof(ChatPrivateId))]
public class ChatPrivateDetail
{
    public string UserId { get; set; }

    public int ChatPrivateId { get; set; }

    public bool IsDeleted { get; set; }
}