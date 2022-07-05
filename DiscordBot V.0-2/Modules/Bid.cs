using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot_V._0_2.Modules
{
    public class Bid
    {
        public List<User> Users { get; set; }
        public int BidMoney { get; set; }
        public string Theme { get; set; }
        public string VarA { get; set; }
        public string VarB { get; set; }
        public ulong MsgId { get; set; }
        public Emoji EmoteA { get; set; }
        public Emoji EmoteB { get; set; }
    }
}
