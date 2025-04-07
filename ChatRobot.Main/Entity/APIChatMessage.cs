namespace ChatRobot.Main.Entity;

public class APIChatMessage
{
    public string role { get; set; } // "system", "user" 或 "assistant"
    public string content { get; set; }
}