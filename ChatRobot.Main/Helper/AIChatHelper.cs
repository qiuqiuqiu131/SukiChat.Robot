using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ChatRobot.Main.Entity;
using ChatServer.Common.Protobuf;
using Microsoft.Extensions.Configuration;

namespace ChatRobot.Main.Helper;

public interface IAIChatHelper
{
    Task<string> GetAIChatMessage(string textMessage);
}

public class AIChatHelper : IAIChatHelper
{
    private readonly HttpClient _httpClient;
    
    private readonly string _apiKey;
    private readonly string _apiUrl;

    public AIChatHelper(IConfigurationRoot configurationRoot)
    {
        _httpClient = new HttpClient();
        
        _apiKey = configurationRoot["API:Key"]!;
        _apiUrl = configurationRoot["API:URL"]!;
    }

    public async Task<string> GetAIChatMessage(string textMessage)
    {
        List<APIChatMessage> _conversationHistory = new List<APIChatMessage>();
        _conversationHistory.Add(new APIChatMessage
        {
            role = "system",
            content = "你是一个有帮助的AI助手。用中文回答用户的问题，回答要简洁专业。语气要温和同时可以适当幽默。"
        });
        _conversationHistory.Add(new APIChatMessage
        {
            role = "user",
            content = textMessage
        });
        
        var request = new APIChatRequest
        {
            model = "deepseek-chat",
            messages = _conversationHistory,
            temperature = 0.7,
            max_tokens = 2000
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsync(_apiUrl, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        
        // 使用JsonDocument或反序列化为强类型对象来解析响应
        using var doc = JsonDocument.Parse(responseJson);
        var completion = doc.RootElement.GetProperty("choices")[0]
            .GetProperty("message").GetProperty("content").GetString();

        return completion;
    }
}