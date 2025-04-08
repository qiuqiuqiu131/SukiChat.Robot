﻿using System.Text;
using ChatServer.Common.Protobuf;
using Google.Protobuf.Collections;

namespace ChatRobot.Main.Tool;

public static class ChatMessageTool
{
    /// <summary>
    /// 加密聊天消息
    /// </summary>
    /// <param name="chatMessages"></param>
    /// <returns></returns>
    public static string EncruptChatMessage(RepeatedField<ChatMessage> chatMessages)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var chatMessage in chatMessages)
        {
            switch (chatMessage.ContentCase)
            {
                case ChatMessage.ContentOneofCase.None: break;
                case ChatMessage.ContentOneofCase.TextMess:
                    stringBuilder.Append((int)chatMessage.ContentCase);
                    stringBuilder.Append(chatMessage.TextMess.Text);
                    stringBuilder.Append("1\n\t3\n\t1\n\t");
                    break;
                case ChatMessage.ContentOneofCase.ImageMess:
                    stringBuilder.Append((int)chatMessage.ContentCase);
                    stringBuilder.Append(chatMessage.ImageMess.FilePath);
                    stringBuilder.Append("__");
                    stringBuilder.Append(chatMessage.ImageMess.FileSize);
                    stringBuilder.Append("1\n\t3\n\t1\n\t");
                    break;
                case ChatMessage.ContentOneofCase.FileMess:
                    stringBuilder.Append((int)chatMessage.ContentCase);
                    stringBuilder.Append(chatMessage.FileMess.FileName);
                    stringBuilder.Append("__");
                    stringBuilder.Append(chatMessage.FileMess.FileSize);
                    stringBuilder.Append("__");
                    stringBuilder.Append(chatMessage.FileMess.FileType);
                    stringBuilder.Append("1\n\t3\n\t1\n\t");
                    break;
                case ChatMessage.ContentOneofCase.SystemMessage:
                    stringBuilder.Append((int)chatMessage.ContentCase);
                    foreach (var systemBlock in chatMessage.SystemMessage.Blocks)
                    {
                        stringBuilder.Append(systemBlock.Text);
                        stringBuilder.Append("__");
                        stringBuilder.Append(systemBlock.Bold ? "1" : "0");
                        stringBuilder.Append("5\n\t7\n\t5\n\t");
                    }

                    break;
                case ChatMessage.ContentOneofCase.CardMess:
                    stringBuilder.Append((int)chatMessage.ContentCase);
                    stringBuilder.Append(chatMessage.CardMess.IsUser ? "1" : "0");
                    stringBuilder.Append("__");
                    stringBuilder.Append(chatMessage.CardMess.Id);
                    stringBuilder.Append("1\n\t3\n\t1\n\t");
                    break;
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 解密聊天消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static RepeatedField<ChatMessage> DecruptChatMessage(string message)
    {
        // 分割消息并过滤空字符串
        List<string> messages = message.Split("1\n\t3\n\t1\n\t")
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();

        RepeatedField<ChatMessage> chatMessages = new RepeatedField<ChatMessage>();

        foreach (var item in messages)
        {
            if (string.IsNullOrEmpty(item)) continue;

            // 获取第一个字符作为类型
            int type = (int)char.GetNumericValue(item[0]);
            string content = item.Substring(1);

            switch ((ChatMessage.ContentOneofCase)type)
            {
                case ChatMessage.ContentOneofCase.TextMess:
                    var textMess = new ChatMessage
                    {
                        TextMess = new TextMess { Text = content }
                    };
                    chatMessages.Add(textMess);
                    break;
                case ChatMessage.ContentOneofCase.ImageMess:
                    string[] image_spliter = content.Split("__");
                    var imageMess = new ChatMessage
                    {
                        ImageMess = new ImageMess
                        {
                            FilePath = image_spliter[0],
                            FileSize = int.Parse(image_spliter[1])
                        }
                    };
                    chatMessages.Add(imageMess);
                    break;
                case ChatMessage.ContentOneofCase.FileMess:
                    string[] file_spliter = content.Split("__");
                    var fileMess = new ChatMessage
                    {
                        FileMess = new FileMess
                        {
                            FileName = file_spliter[0],
                            FileSize = int.Parse(file_spliter[1]),
                            FileType = file_spliter[2]
                        }
                    };
                    chatMessages.Add(fileMess);
                    break;
                case ChatMessage.ContentOneofCase.SystemMessage:
                    string[] system_spliter = content.Split("5\n\t7\n\t5\n\t");
                    SystemMessage systemMessage = new SystemMessage();
                    foreach (var system in system_spliter)
                    {
                        if (string.IsNullOrWhiteSpace(system)) continue;
                        string[] block_spliter = system.Split("__");
                        systemMessage.Blocks.Add(new SystemMessageBlock
                        {
                            Text = block_spliter[0],
                            Bold = block_spliter[1].Equals("0") ? false : true
                        });
                    }

                    chatMessages.Add(new ChatMessage { SystemMessage = systemMessage });
                    break;
                case ChatMessage.ContentOneofCase.CardMess:
                    string[] card_spliter = content.Split("__");
                    var cardMess = new ChatMessage
                    {
                        CardMess = new CardMess
                        {
                            IsUser = card_spliter[0].Equals("1"),
                            Id = card_spliter[1]
                        }
                    };
                    chatMessages.Add(cardMess);
                    break;
            }
        }

        return chatMessages;
    }
}