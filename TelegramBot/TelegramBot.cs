using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot
{
    internal class TelegramBot
    {
        private static readonly ITelegramBotClient bot = new TelegramBotClient("5869879529:AAE_UPJlkhf6efEqwxS2EUbZ4k6ru9bV-QY");

        public static void StartBot()
        {
            var cts = new CancellationTokenSource();

            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message != null)
            {
                string? msg = update.Message.Text;

                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    if (msg == "/start")
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Hi! I'm glad to see you here!\nHere you can see love compatibility by name.\n" +
                            "Be sure to check the compatibility of programmers.", cancellationToken: cancellationToken);
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Write two name: name1 + name2", cancellationToken: cancellationToken);
                    }
                    else if (msg.StartsWith("/joke"))
                    {
                        if (msg.Length <= 6)
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, LoveChecker.GetJoke().Result, cancellationToken: cancellationToken);
                        }
                        else
                        {
                            msg = msg.Replace("+", "%2B");
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, LoveChecker.GetJoke(msg.Substring(6)).Result, cancellationToken: cancellationToken);

                        }
                    }
                    else if (!msg.Contains('+'))
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Uncorrected text🤡", cancellationToken: cancellationToken);
                    }
                    else
                    {
                        string[] names;
                        //string message = update.Message.Text;
                        try
                        {
                            msg = msg.Replace(" ", "");
                            names = msg.Split("+");
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, LoveChecker.GetData(names[0].ToLower(), names[1].ToLower()).Result, cancellationToken: cancellationToken);
                        }
                        catch (Exception e)
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Uncorrected text!");
                            Console.WriteLine(e.Message);
                            throw;
                        }
                    }

                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }



    }
}
