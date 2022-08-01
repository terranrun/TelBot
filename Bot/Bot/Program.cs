using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

        static TelegramBotClient Bot;

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

        private static string ChangeEmployee(string[] name)
        {
            if (name.Length != 4)
                return "неправильный ввод";
            else
            {
                return employee.ChangeEmployee(name[1], name[2], name[3]);

            }

        }
    }


}