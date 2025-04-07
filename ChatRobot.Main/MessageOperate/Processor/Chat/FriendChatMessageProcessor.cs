using ChatRobot.Main.Helper;
using ChatServer.Common.Protobuf;
using Google.Protobuf.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.MessageOperate.Processor.Chat;

public class FriendChatMessageProcessor(IServiceProvider container) : ProcessorBase<FriendChatMessage>(container)
{
    protected override async Task OnProcess(FriendChatMessage message)
    {
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
        await SendMessage(writeMessage);
        
        // 发送消息
        var aiChatHelper = container.GetRequiredService<IAIChatHelper>();
        try
        {
            var result = await aiChatHelper.GetAIChatMessage(message.Messages[0].TextMess.Text);
         
            // 发送停止输入消息
            var stopMessage = new FriendWritingMessage
            {
                IsWriting = false,
                UserFromId = _userManager.UserId,
                UserTargetId = message.UserFromId
            };
            await SendMessage(stopMessage);
            
            // 发送具体消息
            chatMessage.Messages.Add(new ChatMessage
            {
                TextMess = new TextMess{Text = result}
            });
            await SendMessage(chatMessage);
        }
        catch (Exception e)
        {
            // doNothing
        }
    }
}