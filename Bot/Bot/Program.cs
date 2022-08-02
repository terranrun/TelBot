using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot
{
    class Program
    {
        //string iDate = "05/05/2005";
        //DateTime oDate = Convert.ToDateTime(iDate);
        //DateTime.ParseExact(iString, "yyyy-MM-dd HH:mm tt",null)
        /*
         * AddDays(int days): добавляет к дате некоторое количество дней

           AddMonths(int months): добавляет к дате некоторое количество месяцев

           AddYears(int years): добавляет к дате некоторое количество лет
         */

        // сохранить на гите
        // создать метод который редактирует запись
        // разобраться с записью времени (форма записи) 1
        // допилить метод изменения отпуска 1
        // собрать бота 

        //автоматически определять человека в базе данных
        //без ввода имени менять отпуск

        static TelegramBotClient Bot;
        public List<string> _user = null;

        static Employee employee = new Employee();

        // private static CancellationToken HandlerErrorAsync;
        const string COMMAND_LIST =
@"Список команд:
/getInfo - получаем информацию о датах отпуска
/change - меняем дату отпуска введите имя, дату отпуска и количество днейЮ которые хотите потратить на отпуск
(имя дата количество дней) 
";

        private static async Task Main()
        {
            using (var cts = new CancellationTokenSource())
            {

                Bot = new TelegramBotClient("");
                var me = await Bot.GetMeAsync();

                Console.WriteLine($"name is{me.Username}");
                Bot.StartReceiving(HandlerUpdateAsync, HandlerErrorAsync,
                 new ReceiverOptions
                 {
                     AllowedUpdates = Array.Empty<UpdateType>()
                 },
                 cts.Token);

                Console.ReadLine();
                cts.Cancel();
            }

        }


        private static Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            Console.WriteLine(exception);
            return Task.CompletedTask;
        }

        private static async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        await BotOnMessageReceived(botClient, update.Message);
                        break;
                    case (UpdateType.CallbackQuery):
                        await SetBtn(botClient, update.CallbackQuery);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message? message)
        {
            Console.WriteLine($"receive mes type{message.Type}");
            if (message.Type != MessageType.Text)
                return;
            Console.WriteLine(message.Text);
            Console.WriteLine(message.From.Username);
            var userId = message.From.Id;
            var msgArgs = message.Text.Split(' ');
            string text;
            switch (msgArgs[0])
            {
                case "/start":
                    text = COMMAND_LIST;
                    await LoadMenu(botClient, message);
                    break;
                case "/getInfo":
                    text = employee.InfoEmployeeWeekend(msgArgs);
                    break;
                case "/change":
                    text = ChangeEmployee(msgArgs);
                    break;

                default:

                    text = COMMAND_LIST;
                    break;
            }
            await Bot.SendTextMessageAsync(message.From.Id, text);
        }
        private static async Task SetBtn(ITelegramBotClient botClient, CallbackQuery? callbackQuery)
        {
            if (callbackQuery.Data.StartsWith("getInfo"))
            {
                await botClient.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    $"Вы хотите узнать инфу о вашем отпуске?"
                );
                var msgArgs = new string[] { " ", "Mikhail" };
                var text =employee.InfoEmployeeWeekend(msgArgs);
                await Bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text);
                return;
            }
            if (callbackQuery.Data.StartsWith("change"))
            {
                await botClient.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    $"Вы хотите продать?"
                );
                return;
            }
            await botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"You choose with data: {callbackQuery.Data}"
                );
            return;
        }

        private static async Task LoadMenu(ITelegramBotClient botClient, Message message)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                    // first row
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text: "/getInfo", callbackData: "getInfo"),
                        InlineKeyboardButton.WithCallbackData(text: "/change", callbackData: "change"),
                    },

                });

            await botClient.SendTextMessageAsync(message.Chat.Id, "Choose inline:",
                   replyMarkup: inlineKeyboard);
            return;
        }



        //    private static async void Bot_OnMessage(object? sender, MessageEventArgs e)
        //    {
        //        if (e.Message == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
        //            return;
        //        Console.WriteLine(e.Message.Text);
        //        Console.WriteLine(e.Message.From.Username);
        //        var userId = e.Message.From.Id;
        //        var msgArgs = e.Message.Text.Split(' ');
        //        string text;

        //        switch (msgArgs[0])
        //        {
        //            case "/start":
        //                text = COMMAND_LIST;
        //                break;
        //            case "/getInfo":
        //               text = employee.InfoEmployeeWeekend(msgArgs);        
        //                break;
        //            case "/change":
        //                text = ChangeEmployee(msgArgs);
        //                break;
        //            default:
        //                text = COMMAND_LIST;
        //                break;
        //        }
        //       await Bot.SendTextMessageAsync(e.Message.From.Id, text);
        //    }

        async Task OnEnd(ITelegramBotClient botClient, Message message)
        {
            var id = message.Chat.Id.ToString();
            _user.Remove(id);

        }

        async Task OnStart(ITelegramBotClient botClient, Message message)
        {

            var id = message.Chat.Id.ToString();

            if (!_user.Contains(id))
            {
                _user.Add(id);
            }

        }

        //async Task LoadMenu(ITelegramBotClient botClient, Message message)
        //{

        //    InlineKeyboardMarkup inlineKeyboard = new(new[]
        //    {
        //        // first row
        //        new[]
        //        {
        //            InlineKeyboardButton.WithCallbackData(text: "Кнопка 1", callbackData: "post"),
        //            InlineKeyboardButton.WithCallbackData(text: "Кнопка 2", callbackData: "12"),
        //        },

        //    });

        //await botClient.SendTextMessageAsync(message.Chat.Id, "ddd",
        //        replyMarkup: inlineKeyboard);

        //}

        private static string ChangeEmployee(string[] name)
        {
            return name.Length != 4 ? "неправильный ввод" : employee.ChangeEmployee(name[1], name[2], name[3]);

        }
    }


}