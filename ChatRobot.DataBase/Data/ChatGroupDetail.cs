using Microsoft.EntityFrameworkCore;

namespace ChatRobot.DataBase.Data;

[PrimaryKey(nameof(UserId), nameof(ChatGroupId))]
public class ChatGroupDetail
{
    public string UserId { get; set; }

    public int ChatGroupId { get; set; }

    public bool IsDeleted { get; set; }
}