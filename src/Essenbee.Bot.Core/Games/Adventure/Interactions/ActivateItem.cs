﻿namespace Essenbee.Bot.Core.Games.Adventure.Interactions
{
    public class ActivateItem : IAction
    {
        private string _message;

        public ActivateItem(string message)
        {
            _message = message;
        }

        public bool Do(AdventurePlayer player, AdventureItem item)
        {
            item.IsActive = true;
            player.ChatClient.PostDirectMessage(player.Id, _message);

            return true;
        }
    }
}