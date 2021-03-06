﻿using System;
using System.Collections.Generic;
using Essenbee.Bot.Core.Games.Adventure.Interfaces;
using Essenbee.Bot.Core.Interfaces;
using Serilog;

namespace Essenbee.Bot.Web
{
    public class ConsoleChatClient : IChatClient
    {
        public bool UseUsernameForIM { get; }
        public string DefaultChannel => string.Empty;
        public event EventHandler<Core.ChatCommandEventArgs> OnChatCommandReceived = null;

        public IDictionary<string, string> Channels
        {
            get => new Dictionary<string, string> { {"console", "0"} };
            set => throw new NotSupportedException("You cannot set Channels on the ConsoleChatClient");
        }

        public void Connect()
        {
            Console.WriteLine("Connecting to the Console service ...");
            Log.Information("*** Connecting to the Console service ...");
        }

        public void Disconnect()
        {
            Console.WriteLine("Disconnecting from the Console service ...");
            Log.Information("*** Disconnecting from the Console service ...");
        }

        public void PostMessage(string channel, string text)
        {
            Console.WriteLine(text);
            Log.Information("*** " + text);
        }

        public void PostMessage(string text)
        {
            Console.WriteLine(text);
            Log.Information("*** " + text);
        }

        public void PostDirectMessage(string username, string text)
        {
            Console.WriteLine($"<{username}>: " + text);
            Log.Information($"*** <{username}>: " + text);
        }

        public void PostDirectMessage(IAdventurePlayer player, string text) => PostDirectMessage(player.Id, text);
    }
}
