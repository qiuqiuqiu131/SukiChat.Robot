using ChatRobot.Main.Entity;
using ChatRobot.Main.Helper;
using ChatRobot.Main.Service;
using ChatServer.Common.Protobuf;
using Google.Protobuf.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.MessageOperate.Processor.Chat;

public class FriendChatMessageProcessor(IServiceProvider container,IMessageHelper messageHelper,IChatService chatService) : ProcessorBase<FriendChatMessage>(container)
{
    protected override async Task OnProcess(FriendChatMessage message)
    {
        if(message.UserFromId.Equals(_userManager.UserId)) return;
        
        // 保存消息
        await chatService.AddFriendChatMessage(_userManager.UserId,message);
        
        // 拦截消息
        var chatMessage = new FriendChatMessage
            { UserFromId = _userManager.UserId, UserTargetId = message.UserFromId };
        if (message.Messages.Count >= 2)
        {
            chatMessage.Messages.Add(new ChatMessage
            {
                TextMess = new TextMess { Text = "暂不支持接受多条消息" }
            });
            await SendMessage(chatMessage);
            return;
        }
        else if (message.Messages.Count == 1 && message.Messages[0].ContentCase != ChatMessage.ContentOneofCase.TextMess)
        {
            chatMessage.Messages.Add(new ChatMessage
            {
                TextMess = new TextMess { Text = "暂不支持非文本消息" }
            });
            await SendMessage(chatMessage);
            return;
        }

        // 发送正在输入消息
        var writeMessage = new FriendWritingMessage
        {
            IsWriting = true,
            UserFromId = _userManager.UserId,
            UserTargetId = message.UserFromId
        };
        await messageHelper.SendMessage(writeMessage);
        
        // 获取历史记录
        List<APIChatMessage> conversationHistory = _userManager.Robot.System
            .Select(d => new APIChatMessage { content = d, role = "system" }).ToList();
        var chatMessages = await chatService.GetFriendChatMessages(_userManager.UserId, message.UserFromId);
        foreach (var chatMess in chatMessages)
        {
            var role = chatMess.UserTargetId == _userManager.UserId ? "user" : "assistant";
            conversationHistory.AddRange(
                from mess in chatMess.Messages 
                where mess.ContentCase == ChatMessage.ContentOneofCase.TextMess 
                select new APIChatMessage { role = role, content = mess.TextMess.Text });
        }
        conversationHistory.Add(new APIChatMessage
        {
            role = "user",
            content = message.Messages[0].TextMess.Text
        });
        
        // 发送消息
        string? result = null;
        try
        {
            var aiChatHelper = container.GetRequiredService<IAIChatHelper>();
            result = await aiChatHelper.GetAIChatMessage(_userManager.Robot.API.Key,_userManager.Robot.API.URL, _userManager.Robot.Temperature, _userManager.UserId, conversationHistory);
        }
        catch (Exception e)
        {
            return;
        }
        finally
        {
            // 发送停止输入消息
            var stopMessage = new FriendWritingMessage
            {
                IsWriting = false,
                UserFromId = _userManager.UserId,
                UserTargetId = message.UserFromId
            };
            await messageHelper.SendMessage(stopMessage);
        }
        
        // 发送具体消息
        chatMessage.Messages.Add(new ChatMessage
        {
            TextMess = new TextMess { Text = result }
        });
        await SendMessage(chatMessage);
    }

    private async Task SendMessage(FriendChatMessage message)
    {
        var response = await messageHelper.SendMessageWithResponse<FriendChatMessageResponse>(message);
        if (response is { Response: { State: true } })
        {
            message.Id = response.Id;
            message.Time = response.Time;
            
            await chatService.AddFriendChatMessage(_userManager.UserId, message);
        }
    }
}