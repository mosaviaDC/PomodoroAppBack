using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.RegularExpressions;
using System.Net.Mail;
using PomodoroApp.Models;
using Microsoft.Extensions.DependencyInjection;

namespace PomodoroApp.TelegramBot
{
    public class Bot
    {

        private ITelegramBotClient botClient;
        private readonly IServiceScopeFactory scopeFactory;
        private int next = 0;
        public EFDataContext userContext { get; set; }


        public Bot(IServiceScopeFactory ScopeFactory)
        {
            botClient = new TelegramBotClient("1485530612:AAE-Luwocei-Rkv9iCm_oO64qOL93XRRVfA");
            scopeFactory = ScopeFactory;
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {

            var message = e.Message;
            if (message != null)
            {
                int iD = Convert.ToInt32(e.Message.From.Id);
                var keyBoard = new ReplyKeyboardMarkup(new[] {


                                    new  []
                                    {
                                        new KeyboardButton("Привязать аккаунт")

                                    } });

                keyBoard.ResizeKeyboard = true;

                

                switch (message.Text)
                {
                    case "Привязать аккаунт":
                        {
                     
                            await botClient.SendTextMessageAsync(iD, "Напиши e-mail, привязанный к аккаунту");
                            botClient.OnMessage -= Bot_OnMessage;
                            botClient.OnMessage += BotClient_OnMessageRecievedAfter;
                            next = 1;

                        }
                    break;
                    case "/start":
                        {
                            await botClient.SendTextMessageAsync(iD, "Привет, воспользуйся клавиауторой ⌨️ ");
                        }
                     break;
                }



                if (next ==0)
                await botClient.SendTextMessageAsync(e.Message.Chat, "Не понял 🗿", replyMarkup: keyBoard);










            }
        }

        private async void BotClient_OnMessageRecievedAfter(object sender, MessageEventArgs e)
        {
            if (next == 1)
            {

                using (var scope = scopeFactory.CreateScope())
                {
                    EFDataContext userContext = scope.ServiceProvider.GetRequiredService<EFDataContext>();

                    var user = userContext.Users.FirstOrDefault(u => u.Email == e.Message.Text);
                    if (user != null)
                    {


                        user.telegramChatId = (int)e.Message.Chat.Id;
                        user.telegramUserName = e.Message.From.Username;
                        userContext.Users.Update(user);
                        await userContext.SaveChangesAsync();
                        await botClient.SendTextMessageAsync(e.Message.From.Id, "Почта " + e.Message.Text + " добавлена");
                        next = 0;
                        botClient.OnMessage -= BotClient_OnMessageRecievedAfter;
                        botClient.OnMessage += Bot_OnMessage;

                    }
                    else
                    {

                        await botClient.SendTextMessageAsync(e.Message.From.Id, "Пользователь не найден");
                        next = 0;
                        botClient.OnMessage -= BotClient_OnMessageRecievedAfter;
                        botClient.OnMessage += Bot_OnMessage;
                    }
                }
            }
        }

        public async void SendMessage (int chatId,string message)
        {
            
            await botClient.SendTextMessageAsync(chatId, message + "\n" + DateTime.UtcNow.AddHours(3).ToString()+"(МСК)");
        }
    }
}
