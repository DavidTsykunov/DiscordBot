using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.IO;
using System.Xml;
using Victoria;
using DiscordBot_V._0_2.Modules;
using System.Collections.Generic;

namespace DiscordBot_V._0_2
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;
        public static LavaNode lavaNode;
        private LavaConfig config;

        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient();
            config = new LavaConfig();
            commands = new CommandService();
            lavaNode = new LavaNode(client, config);
            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .AddSingleton<LavaNode>()
                .AddSingleton<LavaConfig>()
                .AddLavaNode(x =>
                {
                    x.SelfDeaf = false;

                })
                .BuildServiceProvider();

            
            string token = "token";

            client.Log += Client_Log;
            client.Ready += OnReadyAsync;
            client.ReactionAdded += HandleReactionAsync;
            client.UserJoined += HandleJoinAsync;

            await RegisterCommandsAsync();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        public static List<IUser> usersIdBidA = new List<IUser>();
        public static List<IUser> usersIdBidB = new List<IUser>();

        public async Task HandleReactionAsync(Cacheable<IUserMessage, UInt64> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot) return;

            if (message.Id == Commands.ReturnLastMsgIdBid())
            {
                IUser user = reaction.User.Value;

                Bid bid = Commands.GetLastBid();

                IEmote emoteA = bid.EmoteA;
                IEmote emoteB = bid.EmoteB;

                if (reaction.Emote.Name == emoteA.Name && Commands.GetBalance((IGuildUser)user) >= Commands.ReturnMoneyBid())
                {
                    Commands.AddBalance((IGuildUser)user, Commands.ReturnMoneyBid() * -1);
                    usersIdBidA.Add(user);
                }

                else if (reaction.Emote.Name == emoteB.Name && Commands.GetBalance((IGuildUser)user) >= Commands.ReturnMoneyBid())
                {
                    Commands.AddBalance((IGuildUser)user, Commands.ReturnMoneyBid() * -1);
                    usersIdBidB.Add(user);
                }
            }
        }
        private Task Client_Log(LogMessage arg)
            {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task OnReadyAsync()
        {
            if (!lavaNode.IsConnected)
            {
                await lavaNode.ConnectAsync();
            }
        }
        public async Task HandleCommandAsync(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;
            var context = new SocketCommandContext(client, message);
            if (message.Author.IsBot) return;

            int msgPos = 0;
            if(message.HasStringPrefix("!", ref msgPos))
            {
                var result = await commands.ExecuteAsync(context, msgPos, services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }
            else
            {
                string path = "path";
                if (!File.Exists(path)) new Commands().CreateNewUserXML((IGuildUser)message.Author, path);
                await new Commands().AddXpAsync((IGuildUser)message.Author, "1");
            }
        }

        public async Task HandleJoinAsync(IGuildUser user)
        {
            if (user.IsBot) return;

            string path = "path";

            new Commands().CreateNewUserXML(user, path);
        }

    }
}
