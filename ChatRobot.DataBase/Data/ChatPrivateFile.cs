using Microsoft.EntityFrameworkCore;

namespace ChatRobot.DataBase.Data;

[PrimaryKey(nameof(ChatId), nameof(UserId))]
public class ChatPrivateFile
{
    public int ChatId { get; set; }

    public string UserId { get; set; }

    public string TargetPath { get; set; }
}