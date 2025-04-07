using System.ComponentModel.DataAnnotations;

namespace ChatRobot.DataBase.Data;

public class LoginHistory
{
    [Key]
    public string Id { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public DateTime LastLoginTime { get; set; }
}