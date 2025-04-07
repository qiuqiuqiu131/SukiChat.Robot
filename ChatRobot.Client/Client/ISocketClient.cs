using DotNetty.Transport.Channels;

namespace ChatRobot.Client.Client;

public interface ISocketClient
{
    event Action ConnectedEvent;
    event Action DisconnectedEvent;
    
    IChannel? Channel { get;}
    void Init(SocketClientBuilder builder);
        
    void Start();
    Task Stop();
}