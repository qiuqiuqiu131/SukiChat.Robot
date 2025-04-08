using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.Service;

public abstract class BaseService : IDisposable
{
    protected readonly IServiceScope _scopedProvider;

    public BaseService(IServiceProvider containerProvider)
    {
        _scopedProvider = containerProvider.CreateScope();
        // Console.WriteLine($"create {this.GetType().Name}");
    }

    public void Dispose()
    {
        //Console.WriteLine($"dispose {this.GetType().Name}");
        _scopedProvider.Dispose();
    }
}