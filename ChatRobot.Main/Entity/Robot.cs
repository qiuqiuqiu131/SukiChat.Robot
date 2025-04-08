namespace ChatRobot.Main.Entity;

public class Robot
{
    public Account User { get; set; }
    public APIAddress API { get; set; }
    public List<string> System { get; set; }
    public bool AcceptFriendRequest { get; set; }
}

public class Account
{
    public string ID { get; set; }
    public string Password { get; set; }
}

public class APIAddress
{
    public string Key { get; set; }
    public string URL { get; set; }
}