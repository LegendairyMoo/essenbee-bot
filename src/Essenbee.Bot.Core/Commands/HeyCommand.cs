﻿using Essenbee.Bot.Core.Interfaces;
using Essenbee.Bot.Core.Utilities;
using System;
using System.Threading.Tasks;

namespace Essenbee.Bot.Core.Commands
{
    class HeyCommand : ICommand
    {
        public ItemStatus Status { get; set; } = ItemStatus.Draft;
        public string CommandName => "hey";
        public string HelpText => "Use !hey to get the streamer's attention (Twitch chat only)!";
        public string RestrictToClientType => "twitch";
        public TimeSpan Cooldown { get; }

        private readonly IBot _bot;

        public HeyCommand(IBot bot)
        {
            Cooldown = TimeSpan.FromMinutes(1);
            _bot = bot;
        }

        public Task Execute(IChatClient chatClient, ChatCommandEventArgs e)
        {
            if (Status == ItemStatus.Active)
            {
                if (e.ClientType.ToLower().Contains(RestrictToClientType))
                {
                    Sfx.PlaySound(Sfx.HeyEssenbee);
                }

                chatClient.PostMessage(e.Channel, "**Hey, Essenbee!**");
            }

            return null;
        }

        public bool ShouldExecute()
        {
            return Status == ItemStatus.Active;
        }
    }
}

