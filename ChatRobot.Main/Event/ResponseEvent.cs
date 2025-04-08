using Google.Protobuf;

namespace ChatRobot.Main.Event;

public class ResponseEvent<T> : PubSubEvent<T> where T : IMessage
{
    
}