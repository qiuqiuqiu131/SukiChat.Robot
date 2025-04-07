using ChatRobot.Main;

namespace ChatRobot.Start;

class Program
{
    static void Main(string[] args)
    {
        new RobotHost().Start();
        Console.ReadLine();
    }
}