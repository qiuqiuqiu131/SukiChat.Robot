using ChatServer.Common.Protobuf;

namespace ChatRobot.Main.Entity;

public class APIChatRequest
{
    public string model { get; set; } = "deepseek-chat"; // 使用正确的模型名称
    public List<APIChatMessage> messages { get; set; } = [];
    public double? temperature { get; set; } = 0.7; // 控制回答的随机性(0-2)
    public int? max_tokens { get; set; } = 2000; // 限制响应长度
    public double? top_p { get; set; } = 1.0; // 核采样(0-1)
    public int? n { get; set; } = 1; // 生成多少条回复
    public bool? stream { get; set; } = false; // 是否流式传输
    public string? stop { get; set; } // 停止序列
}