using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ChatRobot.Main.Entity;
using ChatRobot.Main.Manager;
using ChatRobot.Main.Service;
using ChatServer.Common.Protobuf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.Helper;

public interface IAIChatHelper
{
    Task<string> GetAIChatMessage(string apiKey, string apiUrl, string userId, List<APIChatMessage> chatMessage);
}

public class AIChatHelper : IAIChatHelper
{
    private readonly IConfigurationRoot _configurationRoot;
    private readonly HttpClient _httpClient;

    public AIChatHelper(IConfigurationRoot configurationRoot)
    {
        _configurationRoot = configurationRoot;

        _httpClient = new HttpClient();
    }

    public async Task<string> GetAIChatMessage(string apiKey, string apiUrl, string userId,
        List<APIChatMessage> chatMessage)
    {
        var request = new APIChatRequest
        {
            model = "deepseek-chat",
            messages = chatMessage,
            temperature = 1.3,
            max_tokens = 2000
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var response = await _httpClient.PostAsync(apiUrl, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();

        // 使用JsonDocument或反序列化为强类型对象来解析响应
        using var doc = JsonDocument.Parse(responseJson);
        var completion = doc.RootElement.GetProperty("choices")[0]
            .GetProperty("message").GetProperty("content").GetString();

        return completion;
    }
}