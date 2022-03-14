using System;
using System.Collections.Generic;
using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TickServer
{
    public class Telegram
    {
        private readonly TelegramBotClient Bot;
        private static readonly string Base = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string Ty = Base + "log.atlas";
        public List<TelegramUser> ActiveUsers = new List<TelegramUser>();

        private string Name;
       

        public Telegram(string token, string name)
        {
            Bot=new TelegramBotClient(token);
            Name = name;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnReceiveError += BotOnReceiveError;
            Bot.StartReceiving();
        }

        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null) return;
            if (message.Type != MessageType.Text) return;
            if (message.Text.StartsWith("/start") || message.Text.StartsWith("start") || message.Text.StartsWith("Start"))
            {
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                foreach (var telegramUser in ActiveUsers)
                {
                    if (telegramUser.UserId == message.Chat.Id)
                    {
                        SendMessage(message.Chat.Id, $"You and {ActiveUsers.Count - 1} others are connected to {Name}.");
                        return;
                    }
                }
                ActiveUsers.Add(new TelegramUser { UserId = message.Chat.Id });
                SendMessage(message.Chat.Id, "Join the discussion group\r\nhttps://t.me/joinchat/ESvQQRTjieotQA_N8y3Hsw");
            }

        }

        private void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            //throw new NotImplementedException();
        }
        public async void SendMessage(long chatId, string msg)
        {
            try
            {
                Message message = await Bot.SendTextMessageAsync(chatId, msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{Name} Messenger at " + DateTime.Now.ToString("g") + "\n" + ex.Message);
            }
        }
        public async void SendMessage(string msg)
        {
            foreach (var telegramUser in ActiveUsers)
            {
                try
                {
                    Message message = await Bot.SendTextMessageAsync(telegramUser.UserId, msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n{Name} Messenger at " + DateTime.Now.ToString("g") + "\n" + ex.Message);
                }
            }

        }
    }
}
