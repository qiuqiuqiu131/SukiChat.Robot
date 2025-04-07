using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatRobot.DataBase.Data;

public class GroupReceived
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public int RequestId { get; set; }

    [StringLength(10)] public string UserFromId { get; set; }

    [StringLength(10)] public string GroupId { get; set; }

    public DateTime ReceiveTime { get; set; }

    public string Message { get; set; }

    public bool IsAccept { get; set; } = false;

    public bool IsSolved { get; set; } = false;

    public DateTime? SolveTime { get; set; }

    [StringLength(10)] public string? AcceptByUserId { get; set; }
}