﻿using System;
using System.Collections.Generic;
using System.Threading;
using Essenbee.Bot.Core.Interfaces;
using Essenbee.Bot.Core.Messaging;
using Essenbee.Bot.Core.Utilities;
using static System.Console;

namespace Essenbee.Bot.Core
{
    public class Bot
    {
        public List<IChatClient> ConnectedClients { get; }

        public static readonly string DefaultChannel = "general";

        private bool _endProgram = false;
        private readonly AutoMessaging _autoMessaging;

        public Bot(List<IChatClient> connectedClients)
        {
            ConnectedClients = connectedClients ?? throw new ArgumentNullException(nameof(connectedClients));
            _autoMessaging = new AutoMessaging(new SystemClock());
        }

        public void Start()
        {
            Console.CancelKeyPress += OnCtrlC;

            WriteLine();
            WriteLine("Press [Ctrl]+C to exit.");
            WriteLine();

            PublishTimerTriggeredMessages();
            BeginLoop();
        }

        private void PublishTimerTriggeredMessages()
        {
            // ToDo: Eventally, we will want to get these messages from a datastore ...
            var testMsg = new TimerTriggeredMessage
            {
                Delay = 1, // Minutes
                Message = "Hi, this is a timed message from CoreBot!"
            };

            _autoMessaging.PublishMessage(testMsg);
        }

        private void BeginLoop()
        {
            while (true)
            {
                Thread.Sleep(1000);

                // Show Timer Triggered Messages
                _autoMessaging.EnqueueMessagesToDisplay();

                while (true)
                {
                    var (isMessage, message) = _autoMessaging.DequeueNextMessage();

                    if (!isMessage) break;

                    foreach (var client in ConnectedClients)
                    {
                        var channelId = client.Channels.ContainsKey(DefaultChannel)
                            ? client.Channels[DefaultChannel]
                            : string.Empty;

                        client.PostMessage(channelId,
                            $"{DateTime.Now.ToShortTimeString()} - {message}");
                    }
                }

                if (_endProgram) break;
            }
        }

        private void OnCtrlC(object sender, ConsoleCancelEventArgs e)
        {
            foreach (var client in ConnectedClients)
            {
                client.Disconnect();
            }

            _endProgram = true;
        }
    }
}