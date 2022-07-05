using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.IO;
using System.Xml;
using System.Threading;
using System.Collections.Immutable;
using Victoria;
using Victoria.Enums;
using Discord.Audio;
using DiscordBot_V._0_2;
using System.Diagnostics;

namespace DiscordBot_V._0_2.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        static List<Bid> Bids = new List<Bid>();


        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У тебя нет парв ``админ``-а.")]
        public async Task Ban(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Пользователь не указан!");
                return;
            }
            if (reason == null) reason = "не указанна";

            await Context.Guild.AddBanAsync(user, 1, reason);                                                               //Пользователь(которого баним), кол-во дней, причина

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark: {user.Mention} забанен. \nПо прочине: {reason}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User Ban Log")
                    .WithIconUrl("https://www.sportspng.com/images/25/5fd8c1c6d0fc2.png");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(886126713355005963) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder()
               .WithDescription($"Забанен: {user.Mention}\nПо прочине: {reason}\nАдмином: {Context.User.Mention}")
               .WithFooter(footer =>
               {
                   footer
                   .WithText("User Ban Log")
                   .WithIconUrl("https://www.sportspng.com/images/25/5fd8c1c6d0fc2.png");
               });
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);
        }

        [Command("unban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У тебя нет парв ``админ``-а.")]
        public async Task Unban(IGuildUser user = null)
        {
            if (user == null)
            {
                await ReplyAsync("Вы не указали пользователя");
                return;
            }

            await Context.Guild.RemoveBanAsync(user);

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark: {user.Mention} разабанен. ")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User Ban Log")
                    .WithIconUrl("https://www.sportspng.com/images/25/5fd8c1c6d0fc2.png");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(886126713355005963) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder()
               .WithDescription($"Разбанен: {user.Mention}\nАдмином: {Context.User.Mention}")
               .WithFooter(footer =>
               {
                   footer
                   .WithText("User Ban Log")
                   .WithIconUrl("https://www.sportspng.com/images/25/5fd8c1c6d0fc2.png");
               });
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);
        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У тебя нет парв ``админ``-а.")]
        public async Task Kick(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Неправильно веденна команда.");
                return;
            }
            if (reason == null) reason = "не указана";

            await user.KickAsync(reason);

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark: {user.Mention} кикнут. ")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User Ban Log")
                    .WithIconUrl("https://www.sportspng.com/images/25/5fd8c1c6d0fc2.png");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(886126713355005963) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder()
               .WithDescription($"Кикнут: {user.Mention}\nПо причине: {reason}\nАдмином: {Context.User.Mention}")
               .WithFooter(footer =>
               {
                   footer
                   .WithText("User Ban Log")
                   .WithIconUrl("https://www.sportspng.com/images/25/5fd8c1c6d0fc2.png");
               });
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);
        }

        [Command("addLvl")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У тебя нет парв ``админ``-а.")]
        public async Task AddLvlAsync(IGuildUser user, [Remainder] string lvl = null)
        {
string path = "path";

            int l = Convert.ToInt32(lvl);
            AddLvl(user, l);

            User u = CreateUser(path);

            if (!File.Exists(path)) CreateNewUserXML(user, path);

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark: Лвл {user.Mention} повышее.\n Lvl: {u.Lvl}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Bank")
                    .WithIconUrl("https://www.sportspng.com/images/25/5fd8c1c6d0fc2.png");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        [Command("addMoney")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У тебя нет парв ``админ``-а.")]
        public async Task AddMoneyAsync(IGuildUser user = null, [Remainder] string balance = null)
        {
string path = "path";

            int b = Convert.ToInt32(balance);
            AddBalance(user, b);

            path += $"{user.Id}.xml";
            User u = CreateUser(path);

            if (!File.Exists(path)) CreateNewUserXML(user, path);

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark: Баланс {user.Mention} пополнен.\n Баланс: {u.Balance}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Bank")
                    .WithIconUrl("https://www.sportspng.com/images/25/5fd8c1c6d0fc2.png");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }


        [Command("balance")]
        public async Task Balance()
        {
            IGuildUser user = (IGuildUser)Context.User;
string path = "path";

            if (!File.Exists(path)) CreateNewUserXML(user, path);

            User u = CreateUser(path);

            await ReplyAsync($"Баланс: {u.Balance}");
        }

        [Command("addXp")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У тебя нет парв ``админ``-а.")]
        public async Task AddXpAsync(IGuildUser user, [Remainder] string xp = null)
        {

            int x = Convert.ToInt32(xp);
            AddXp(user, x);

string path = "path";
            User u = CreateUser(path);

            if (!File.Exists(path)) CreateNewUserXML(user, path);

            return;
        }

        public static int GetBalance(IGuildUser user)
        {
            
string path = "path";

            if (!File.Exists(path)) new Commands().CreateNewUserXML(user, path);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {

                    if (childnode.Name == "Balance")
                    {
                        int b = Int32.Parse(childnode.InnerText);
                        return b;
                    }
                }
            }

            return 0;          
        }

        public static void AddLvl(IGuildUser user, int lvl)
        {
            if (lvl < 0) lvl *= -1;
string path = "path";

            if (!File.Exists(path))new Commands().CreateNewUserXML(user, path);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "Lvl")
                    {
                        int l = Int32.Parse(childnode.InnerText);
                        l += lvl;
                        childnode.InnerText = l.ToString();
                    }
                }
            }

            xDoc.Save(path);
        }

        public static void AddBalance(IGuildUser user, int balance)
        {
string path = "path";

            if (!File.Exists(path))new Commands().CreateNewUserXML(user, path);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {

                    if (childnode.Name == "Balance")
                    {
                        int b = Int32.Parse(childnode.InnerText);
                        b += balance;
                        childnode.InnerText = b.ToString();
                    }
                }
            }

            xDoc.Save(path);
        }

        public static void AddXp(IGuildUser user, int xp)
        {
string path = "path";

            if (!File.Exists(path)) new Commands().CreateNewUserXML(user, path);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlElement xRoot = xDoc.DocumentElement;

            int lvl = 0;

            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {

                    if (childnode.Name == "Lvl")
                    {
                        lvl = Int32.Parse(childnode.InnerText);
                    }

                    if (childnode.Name == "Xp")
                    {
                        int x = Int32.Parse(childnode.InnerText);
                        x += xp;
                        while(x >= lvl * 10 * lvl)
                        { 
                            AddLvl(user, 1);
                            x -= lvl * 10 * lvl;
                            ++lvl;
                            xDoc.Load(path);
                        }
                        childnode.InnerText = x.ToString();
                        xDoc.Save(path);
                    }
                }
            }
            xDoc.Load(path);
            xDoc.Save(path);
        }

        public void CreateNewUserXML(IGuildUser user, string path)
        {
            using (var stream = File.Create(path)) { stream.Close(); }

            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sw.WriteLine("<Users>");
                sw.WriteLine("</Users>");

                sw.Close();
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);

            XmlElement xRoot = xDoc.DocumentElement;

            XmlElement userElem = xDoc.CreateElement("User");
            XmlElement nicknameElem = xDoc.CreateElement("Nickname");
            XmlElement idElem = xDoc.CreateElement("Id");
            XmlElement balanceElem = xDoc.CreateElement("Balance");
            XmlElement lvlElem = xDoc.CreateElement("Lvl");
            XmlElement xpElem = xDoc.CreateElement("Xp");

            XmlText nicknameText = xDoc.CreateTextNode(user.Username);
            XmlText idText = xDoc.CreateTextNode(user.Id.ToString());
            XmlText balanceText = xDoc.CreateTextNode("0");
            XmlText lvlText = xDoc.CreateTextNode("1");
            XmlText xpText = xDoc.CreateTextNode("0");

            nicknameElem.AppendChild(nicknameText);
            idElem.AppendChild(idText);
            balanceElem.AppendChild(balanceText);
            lvlElem.AppendChild(lvlText);
            xpElem.AppendChild(xpText);
            userElem.AppendChild(idElem);
            userElem.AppendChild(nicknameElem);
            userElem.AppendChild(balanceElem);
            userElem.AppendChild(lvlElem);
            userElem.AppendChild(xpElem);

            xRoot.AppendChild(userElem);
            xDoc.Save(path);
        }

        private User CreateUser(string path)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlElement xRoot = xDoc.DocumentElement;

            User user = new User();

            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "Id")
                        user.Id = Convert.ToUInt64(childnode.InnerText);

                    if (childnode.Name == "Nickname")
                        user.Nickname = childnode.InnerText;

                    if (childnode.Name == "Balance")
                        user.Balance = Int32.Parse(childnode.InnerText);

                    if (childnode.Name == "Lvl")
                        user.Lvl = Int32.Parse(childnode.InnerText);

                    if (childnode.Name == "Xp")
                        user.Xp = Int32.Parse(childnode.InnerText);
                }
            }

            return user;
        }

        [Command("spam")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "Такой команды нет. :shushing_face: ")]
        public async Task Spam(IGuildUser user, string spam)
        {
            for (int i = 0; i < 100; i++)
            {
                await user.SendMessageAsync(spam);
            }
            await user.SendMessageAsync("Извини :point_right: :point_left: :pleading_face:");
        }

        [Command("1000-7")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "Такой команды нет. :shushing_face: ")]
        public async Task Ghoul(IGuildUser user)
        {
            for (int i = 1000; i >= 0; i -= 7)
            {
                await user.SendMessageAsync($"{i} - 7 = {i - 7}");
            }
            await user.SendMessageAsync("???");
        }

        [Command("mute")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У тебя нет парв ``админ``-а.")]
        public async Task Mute(IGuildUser user, string time)
        {
            var arr = Context.Guild.Roles.ToImmutableArray();
            bool isHave = false;
            int indexRole = 0;
            ulong roleId = 886570579270991922;

            for (int i = 0; i < arr.Length; i++)
                if (arr[i].Name == "Mute")
                {
                    isHave = true;
                    indexRole = i;
                    break;
                }
            if (!isHave)
            {

                var role = await Context.Guild.CreateRoleAsync("Mute", Discord.GuildPermissions.None, Color.DarkBlue, false, options: null);
                roleId = role.Id;
                await user.AddRoleAsync(role);

                foreach (var channel in Context.Guild.Channels)
                {
                    await channel.AddPermissionOverwriteAsync(role,
                    OverwritePermissions.DenyAll(channel).Modify(
                    viewChannel: PermValue.Allow, readMessageHistory: PermValue.Allow)
                    );
                }
            }
            else
            {
                roleId = arr[indexRole].Id;
                var role = Context.Guild.GetRole(roleId);
                await user.AddRoleAsync(role);

            }

            int sec = 0;
            int min = 0;
            int h = 0;
            if (time.Contains('s')) sec = Convert.ToInt32(time.Substring(0, time.Length - 1));
            if (time.Contains('m')) min = Convert.ToInt32(time.Substring(0, time.Length - 1));
            if (time.Contains('h')) h = Convert.ToInt32(time.Substring(0, time.Length - 1));

            await Task.Delay((sec * 1000) + (min * 60000) + (h * 3600000));
            await user.RemoveRoleAsync(roleId);

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark: {user.Mention} замучен.")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User Ban Log")
                    .WithIconUrl("https://www.sportspng.com/images/25/5fd8c1c6d0fc2.png");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(886126713355005963) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder()
               .WithDescription($"Замучен: {user.Mention}\nВремя: {time}\nАдмином: {Context.User.Mention}")
               .WithFooter(footer =>
               {
                   footer
                   .WithText("User Ban Log")
                   .WithIconUrl("https://www.sportspng.com/images/25/5fd8c1c6d0fc2.png");
               });
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);
        }

        [Command("p")]
        public async Task PlayAsync([Remainder] string query)
        {
            ulong channelId = 788672289801633812;
            ITextChannel channel = Context.Client.GetChannel(channelId) as ITextChannel;

            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await channel.SendMessageAsync("Куда мне заходить то?");
                return;
            }

            try
            {
                await DiscordBot_V._0_2.Program.lavaNode.JoinAsync(voiceState.VoiceChannel, Context.Channel as ITextChannel);
            }
            catch (Exception exception)
            {
                await channel.SendMessageAsync(exception.Message);
            }

            var searchResponse = await DiscordBot_V._0_2.Program.lavaNode.SearchYouTubeAsync(query);
            if (searchResponse.LoadStatus == LoadStatus.LoadFailed ||
                searchResponse.LoadStatus == LoadStatus.NoMatches)
            {
                await channel.SendMessageAsync($"Я ничё не понял вот тута `{query}`.");
                return;
            }

            var player = DiscordBot_V._0_2.Program.lavaNode.GetPlayer(Context.Guild);

            if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
            {
                var track = searchResponse.Tracks[0];
                player.Queue.Enqueue(track);

                var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":musical_note: Добавлен в очередь пользователем {Context.User.Username}.\n" +
                $"``Треков в очереди:`` {player.Queue.Count + 1}\n" +
                $"\t``Треки:``\n" +
                $"{QueueList(player)}")
                .WithTitle($"{track.Title}")
                .WithColor(Color.Purple)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Music")
                    .WithIconUrl($"{searchResponse.Tracks[0].Url}");
                });
                Embed embed = EmbedBuilder.Build();
                await channel.SendMessageAsync(embed: embed);
            }
            else
            {
                var track = searchResponse.Tracks[0];
                await player.PlayAsync(track);
                var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":musical_note: Добавлен в очередь пользователем {Context.User.Username}.\n" +
                $"\t``Сейчас играет:`` {player.Track.Title}\n" +
                $"``Треков в очереди:`` {player.Queue.Count + 1}\n" +
                $"\t``Треки:``\n" +
                $"{QueueList(player)}")
                .WithTitle($"{track.Title}")
                .WithColor(Color.Purple)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Music")
                    .WithIconUrl($"{searchResponse.Tracks[0].Url}");
                });
                Embed embed = EmbedBuilder.Build();
                await channel.SendMessageAsync(embed: embed);
                await NextMusic(player);
            }
        }

        private async Task NextMusic(LavaPlayer player)
        {
            if(player.PlayerState == PlayerState.Stopped)
            {
                await player.PlayAsync(player.Queue.Peek());
            }
        }
        private string QueueList(LavaPlayer player)
        {
            DefaultQueue<LavaTrack> queue = player.Queue;
            ImmutableList<LavaTrack> list = queue.ToImmutableList();

            string str = null;
            str += $"> 1. { player.Track.Title}\n";
            for (int i = 0; i < player.Queue.Count; i++)
            {
                str += $"> {i + 2}. {list[i].Title}.\n";
            }
            return str;
        }

        [Command("skip")]
        public async Task Skip()
        {
            ulong channelId = 1111111111;           //channelId
            ITextChannel channel = Context.Client.GetChannel(channelId) as ITextChannel;

            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await channel.SendMessageAsync("Нечего скипать");
                return;
            }

            if (!DiscordBot_V._0_2.Program.lavaNode.HasPlayer(Context.Guild))
            {
                await channel.SendMessageAsync("Я не подключён к каналу. Накосячил");
                return;
            }

            var player = DiscordBot_V._0_2.Program.lavaNode.GetPlayer(Context.Guild);

            if (voiceState.VoiceChannel != player.VoiceChannel)
            {
                await channel.SendMessageAsync("Я в другом канале. Накосячил");
                return;
            }

            if (player.Queue.Count == 0)
            {
                if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
                {
                    await player.StopAsync();
                    if (player.Queue.Peek() != null) await player.PlayAsync(player.Queue.Peek());
                    await channel.SendMessageAsync("Скипнут.");
                    return;
                }
                await channel.SendMessageAsync("Нет треков в очереди.");
                return;
            }

            await player.SkipAsync();
            await channel.SendMessageAsync("Скипнут.");
        }

        [Command("queue")]
        public async Task Queue()
        {
            ulong channelId = 788672289801633812;
            ITextChannel channel = Context.Client.GetChannel(channelId) as ITextChannel;

            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await channel.SendMessageAsync("Мужик, а где?");
                return;
            }

            if (!DiscordBot_V._0_2.Program.lavaNode.HasPlayer(Context.Guild))
            {
                await channel.SendMessageAsync("Я не подключён к каналу. Накосячил");
                return;
            }

            var player = DiscordBot_V._0_2.Program.lavaNode.GetPlayer(Context.Guild);

            if(player.PlayerState == PlayerState.Playing && player.Queue.Count == 0)
            {
                await channel.SendMessageAsync("Очередь пуста.");
            }

            if (voiceState.VoiceChannel != player.VoiceChannel)
            {
                await channel.SendMessageAsync("Я в другом канале. Накосячил");
                return;
            }

            var EmbedBuilder = new EmbedBuilder()
               .WithDescription($"``Треков в очереди:`` {player.Queue.Count + 1}\n" +
               $"\t``Треки:``\n" +
               $"{QueueList(player)}")
               .WithTitle($":musical_note: Queue: ")
               .WithColor(Color.Purple)
               .WithFooter(footer =>
               {
                   footer
                   .WithText("Music");
               });
            Embed embed = EmbedBuilder.Build();
            await channel.SendMessageAsync(embed: embed);
        }

        [Command("stop")]
        public async Task Stop()
        {
            ulong channelId = 788672289801633812;
            ITextChannel channel = Context.Client.GetChannel(channelId) as ITextChannel;

            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await channel.SendMessageAsync("Мужик, чё мне скипать то?");
                return;
            }

            if (!DiscordBot_V._0_2.Program.lavaNode.HasPlayer(Context.Guild))
            {
                await channel.SendMessageAsync("Я не подключён к каналу. Накосячил");
                return;
            }

            var player = DiscordBot_V._0_2.Program.lavaNode.GetPlayer(Context.Guild);

            if (voiceState.VoiceChannel != player.VoiceChannel)
            {
                await channel.SendMessageAsync("Я в другом канале. Накосячил");
                return;
            }

            if (player.Queue.Count == 0)
            { 
                await channel.SendMessageAsync("Нечего останавливать.");
                return;
            }

            await player.StopAsync();
            player.Queue.Clear();
            await channel.SendMessageAsync("Очередь очищенна, музыка остановленна.");
        }

        [Command("profile")]
        public async Task Profile()
        {
            IGuildUser user = (IGuildUser)Context.User;
string path = "path";

            ulong channelId = 788672289801633812;
            ITextChannel channel = Context.Client.GetChannel(channelId) as ITextChannel;

            if (!File.Exists(path)) CreateNewUserXML(user, path);

            User u = CreateUser(path);

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($"``LvL:`` {u.Lvl}.\n" +
                $"``Xp:`` {u.Xp}/{u.Lvl*10*u.Lvl}\n" +
                $"``Balance:`` {u.Balance}")
                .WithTitle($":notepad_spiral: {user.Username}")
                .WithColor(Color.Purple)
                .WithImageUrl(user.GetAvatarUrl());
            Embed embed = EmbedBuilder.Build();
            await channel.SendMessageAsync(embed: embed);
        }

        static public Bid GetLastBid()
        {
            return Bids[Bids.Count - 1];
        }

        static public ulong ReturnLastMsgIdBid()
        {
            return Bids[Bids.Count - 1].MsgId;
        }

        static public int ReturnMoneyBid()
        {
            return Bids[Bids.Count - 1].BidMoney;
        }

        [Command("createBid")]
        public async Task CreateBid(string theme, string varA, string varB, string money, string time)
        {
            IGuildUser user = (IGuildUser)Context.Message.Author;

            int sec = 0;
            int min = 0;
            int h = 0;
            if (time.Contains('s')) sec = Convert.ToInt32(time.Substring(0, time.Length - 1));
            if (time.Contains('m')) min = Convert.ToInt32(time.Substring(0, time.Length - 1));
            if (time.Contains('h')) h = Convert.ToInt32(time.Substring(0, time.Length - 1));         

            ulong channelId = 788672289801633812;
            ITextChannel channel = Context.Client.GetChannel(channelId) as ITextChannel;

            var EmbedBuilder = new EmbedBuilder()
               .WithDescription($"1. {varA}.\n2. {varB}.")
               .WithTitle($":dollar: Ставка: {theme} ({money})")
               .WithColor(Color.Purple)
               .WithImageUrl(user.GetAvatarUrl());
            Embed embed = EmbedBuilder.Build();

            IMessage msg = await channel.SendMessageAsync(embed: embed);

            Bids.Add(new Bid()
            {  
                BidMoney = Convert.ToInt32(money),
                MsgId = msg.Id,           
                Theme = theme,
                VarA = varA,
                VarB = varB,
                EmoteA = new Emoji("1️⃣"),
                EmoteB = new Emoji("2️⃣") 
            });
   

            await msg.AddReactionAsync(Bids[Bids.Count - 1].EmoteA);
            await msg.AddReactionAsync(Bids[Bids.Count - 1].EmoteB);

            await Task.Delay((sec * 1000) + (min * 60000) + (h * 3600000));
            await msg.DeleteAsync();

            var EmbedBuilderA = new EmbedBuilder()
               .WithDescription($"Завершенна.")
               .WithTitle($":dollar: Ставка: {theme}")  
               .WithColor(Color.Purple)
               .WithImageUrl(user.GetAvatarUrl());
            Embed embedA = EmbedBuilderA.Build();

            await channel.SendMessageAsync(embed: embedA);
        } 

        [Command("winBid")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У тебя нет парв ``админ``-а.")]
        public async Task WinBid(string trueVar)
        {
            int trueVariation = Convert.ToInt32(trueVar);
            List<IUser> users = new List<IUser>();

            if (trueVariation == 1)
            {                
                for(int i = 0; i < Program.usersIdBidA.Count; i++)
                {
                    users.Add(Program.usersIdBidA[i]);
                    AddBalance((IGuildUser)users[i], ReturnMoneyBid() * 2);
                }
            }
            else if (trueVariation == 2)
            {
                for (int i = 0; i < Program.usersIdBidB.Count; i++)
                {
                    users.Add(Program.usersIdBidB[i]);
                    AddBalance((IGuildUser)users[i], ReturnMoneyBid() * 2);
                }
            }

            ulong channelId = 788672289801633812;
            ITextChannel channel = Context.Client.GetChannel(channelId) as ITextChannel;

            if (users.Count > 0)
            {
                var EmbedBuilder = new EmbedBuilder()
                   .WithDescription($"Ставку выиграли:\n{WhoWinBid(users)}")
                   .WithTitle($":dollar: Ставка: {Bids[Bids.Count - 1].Theme} ({Bids[Bids.Count - 1].BidMoney})")
                   .WithColor(Color.Purple);
                Embed embed = EmbedBuilder.Build();

                await channel.SendMessageAsync(embed: embed);
            }
            else
            {
                var EmbedBuilder = new EmbedBuilder()
                   .WithDescription($"Ставку никто не выиграл(\n{WhoWinBid(users)}")
                   .WithTitle($":dollar: Все деньги ушли в банк.")
                   .WithColor(Color.Purple);
                Embed embed = EmbedBuilder.Build();

                await channel.SendMessageAsync(embed: embed);
            }

            Program.usersIdBidA = new List<IUser>();
            Program.usersIdBidB = new List<IUser>();
        }

        private string WhoWinBid(List<IUser> users)
        {
            string str = null;
            for(int i = 0; i < users.Count; i++)
            {
                str += $"{i+1}. {users[i].Username}\n";
            }
            return str;
        }
    }
}