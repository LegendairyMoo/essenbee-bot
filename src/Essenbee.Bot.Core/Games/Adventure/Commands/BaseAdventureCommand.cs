﻿using System.Collections.Generic;
using System.Linq;

namespace Essenbee.Bot.Core.Games.Adventure.Commands
{
    public abstract class BaseAdventureCommand : IAdventureCommand
    {
        protected List<string> Verbs;
        protected readonly IReadonlyAdventureGame _game;

        protected BaseAdventureCommand(IReadonlyAdventureGame game, params string[] verbs)
        {
            _game = game;
            Verbs = new List<string>(verbs);
        }

        public abstract void Invoke(AdventurePlayer player, ChatCommandEventArgs e);

        public virtual bool IsMatch(string verb)
        {
            return Verbs.Any(v => verb.Equals(v));
        }
    }
}