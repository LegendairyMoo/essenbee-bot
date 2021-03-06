﻿using Essenbee.Bot.Core.Games.Adventure.Interfaces;
using Essenbee.Bot.Core.Games.Adventure.Items;
using System.Collections.Generic;

namespace Essenbee.Bot.Core.Games.Adventure.Locations
{
    public class Valley : AdventureLocation
    {
        public Valley(IReadonlyAdventureGame game) : base(game)
        {
            LocationId = Location.Valley;
            Name = "Valley";
            ShortDescription = "in a valley";
            LongDescription = "in a valley in the forest beside a stream tumbling along a rocky bed.";
            WaterPresent = true;
            Items = new List<IAdventureItem> { ItemFactory.GetInstance(Game, Item.Water) };
            ValidMoves = new List<IPlayerMove> {
                new PlayerMove(string.Empty, Location.Forest, "forest", "west", "w", "up", "u", "east", "e"),
                new PlayerMove(string.Empty, Location.Slit, "downstream", "south", "s"),
                new PlayerMove(string.Empty, Location.Road, "upstream", "n", "north"),
            };

        }
    }
}
