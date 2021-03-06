﻿using Essenbee.Bot.Core.Games.Adventure.Interfaces;

namespace Essenbee.Bot.Core.Games.Adventure.Interactions
{
    public class ActivateItem : IAction
    {
        private string _message;

        public ActivateItem(string message)
        {
            _message = message;
        }

        public bool Do(IAdventurePlayer player, IAdventureItem item)
        {
            item.IsActive = true;
            player.ChatClient.PostDirectMessage(player, _message);

            return true;
        }
    }
}
