using ChatRobot.Main;
using ChatRobot.Main.Entity;
using Microsoft.Extensions.Configuration;

namespace ChatRobot.Start;

class Program
{
    static void Main(string[] args)
    {
        IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            .AddJsonFile("usersettings.json", optional: true, reloadOnChange: true)
            .Build();
        var robots = configurationRoot.GetSection("Robot").Get<List<Robot>>();
        foreach (var robot in robots)
        {
            Task.Run(() =>
            {
                try
                {
                    new RobotHost(robot).Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }
        Console.ReadLine();
    }
}