﻿using System.Collections.Generic;
using Essenbee.Bot.Core.Games.Adventure.Locations;

namespace Essenbee.Bot.Core.Games.Adventure.Interfaces
{
    public interface IAdventureLocation
    {
        IReadonlyAdventureGame Game { get; }
        bool IsDark { get; set; }
        IList<IAdventureItem> Items { get; set; }
        Location LocationId { get; set; }
        string LongDescription { get; set; }
        string Name { get; set; }
        string ShortDescription { get; set; }
        IList<IPlayerMove> ValidMoves { get; set; }
        bool WaterPresent { get; set; }
        bool IsStart { get; set; }

        void AddItem(IAdventureItem item);
        void AddMoves(List<IPlayerMove> newMoves);
        int Count();
        void RemoveDestination(Location destination);
        void RemoveItem(IAdventureItem item);
    }
}