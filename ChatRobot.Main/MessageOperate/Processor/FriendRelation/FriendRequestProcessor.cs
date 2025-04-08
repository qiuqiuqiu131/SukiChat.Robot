using ChatRobot.Main.Helper;
using ChatRobot.Main.Service;
using ChatServer.Common.Protobuf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main.MessageOperate.Processor.FriendRelation;

public class FriendRequestProcessor(IServiceProvider container,IMessageHelper messageHelper,IConfigurationRoot configurationRoot)
    : ProcessorBase<FriendRequestFromServer>(container)
{
    protected override async Task OnProcess(FriendRequestFromServer message)
    {
        if (!_userManager.Robot.AcceptFriendRequest)
            return;

        await Task.Delay(2000);

        // 发送添加好友请求
        var friendResponse = new FriendResponseFromClient
        {
            Group = "默认分组",
            Accept = true,
            Remark = "",
            RequestId = message.RequestId,
            ResponseTime = DateTime.Now.ToString()
        };
        await messageHelper.SendMessageWithResponse<FriendResponseFromClientResponse>(friendResponse);
    }
}