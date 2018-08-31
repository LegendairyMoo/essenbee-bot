﻿using Essenbee.Bot.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Essenbee.Bot.Core.Games.Adventure.Locations;

namespace Essenbee.Bot.Core.Games.Adventure
{
    public class AdventurePlayer
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public AdventureLocation CurrentLocation { get; set; }
        public IChatClient ChatClient { get; set; }
        public int Score { get; set; }
        public int Moves { get; set; }
        public Inventory Inventory { get; set; }
        public IList<PlayerStatus> Statuses { get; set; }

        public AdventurePlayer()
        {
            Inventory = new Inventory();
            Statuses = new List<PlayerStatus>();
        }

        public AdventurePlayer(string userId, string userName, IChatClient chatClient)
        {
            Inventory = new Inventory();
            Statuses = new List<PlayerStatus>();
            Id = userId;
            UserName = userName;
            Score = 0;
            Moves = 0;
            ChatClient = chatClient;
        }
    }
}
