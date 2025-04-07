using Google.Protobuf;

namespace ChatRobot.Main.MessageOperate
{
    public interface IProcessor<in T> where T : IMessage
    {
        Task Process(T message);
    }
}
