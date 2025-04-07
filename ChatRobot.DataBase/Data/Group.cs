using System.ComponentModel.DataAnnotations;

namespace ChatRobot.DataBase.Data;

public class Group
{
    [Key] [StringLength(10)] public string Id { get; set; }

    [StringLength(30)] public string Name { get; set; }

    [StringLength(100)] public string? Description { get; set; }

    public DateTime CreateTime { get; set; }

    public int HeadIndex { get; set; } = 1;

    public bool IsCustomHead { get; set; } = false;

    public bool IsDisband { get; set; } = false;
}