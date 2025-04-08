using ChatRobot.Main.Helper;
using ChatRobot.Main.Service;
using ChatServer.Common.Protobuf;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.MessageOperate.Processor.FriendRelation;

public class NewFriendMessageProcessor(IServiceProvider container,IMessageHelper messageHelper) : ProcessorBase<NewFriendMessage>(container)
{
    protected override async Task OnProcess(NewFriendMessage message)
    {
        await Task.Delay(5000);
        var friendChat = new FriendChatMessage
        {
            UserFromId = _userManager.UserId,
            UserTargetId = message.FrinedId,
            Time = DateTime.Now.ToString()
        };
        friendChat.Messages.Add(new ChatMessage
        {
            TextMess = new TextMess { Text = $"你好，我是{_userManager.User.Name},很高兴认识你！" },
        });

        var result = await messageHelper.SendMessageWithResponse<FriendChatMessageResponse>(friendChat);
        
        if(result is not {Response:{State:true}}) return;
        friendChat.Id = result.Id;
        var chatService = container.GetRequiredService<IChatService>();
        await chatService.AddFriendChatMessage(_userManager.UserId, friendChat);
    }
}