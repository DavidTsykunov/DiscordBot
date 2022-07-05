using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBot_V._0_1
{
    class Program
    {
        DiscordSocketClient client;                 //Клиент клиент-серверного проекта, для бота Discord
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            client = new DiscordSocketClient();
            client.MessageReceived += CommandsHandler;
            client.Log += Log;                                          //Добавление логгирования для бота

            string token = "ODg1OTYyNjIwNjEwNDQxMjE2.YTuq_Q.O665mUBu7pVwIN0XNVRSRqOMbEk";

            await client.LoginAsync(TokenType.Bot, token);              //Вход бота в Discord
            await client.StartAsync();                                  //Запуск бота;

            Console.ReadKey();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task CommandsHandler(SocketMessage msg)
        {
            if(! msg.Author.IsBot)                                  //Проверка кто автор сообщение, если бот, то условие не срабатывает
                switch (msg.Content)
                {
                    case "!Привет":
                        {
                            msg.Channel.SendMessageAsync($"Привет, {msg.Author}");          //Отправка имени при соответствии с условием
                            msg.Author.SendMessageAsync($"Привет, {msg.Author.Mention}");
                            break;
                        }
                    case "!spam":
                        {

                            break;
                        }
                }
                
                //msg.Channel.SendMessageAsync(msg.Content);              //Отправка сообщения в тот же чат, с тем же текстом
            return Task.CompletedTask;
        }

        private async Task Ban(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null) ;
        }
    }
}
