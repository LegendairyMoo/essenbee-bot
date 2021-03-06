﻿using System;
using Essenbee.Bot.Core.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace Essenbee.Bot.Core.Commands
{
    public class CricketCommand : ICommand
    {
        public ItemStatus Status { get; set; } = ItemStatus.Draft;
        public string CommandName { get => "cricket"; }
        public string HelpText { get; } = @"The !cricket command shows live scoreboards from around the world.";
        public TimeSpan Cooldown { get; }

        private readonly IBot _bot;

        public CricketCommand(IBot bot)
        {
            _bot = bot;
            Cooldown = TimeSpan.FromMinutes(1);
        }

        public Task Execute(IChatClient chatClient, ChatCommandEventArgs e)
        {
            if (Status == ItemStatus.Active)
            {
                var text = string.Empty;
                var webClient = new WebClient();

                var cricXML = webClient.DownloadString("http://static.cricinfo.com/rss/livescores.xml");

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(cricXML);

                var itemsRemaining = 10;

                foreach (System.Xml.XmlNode xmlNode in xmlDoc.SelectNodes("rss/channel/item"))
                {
                    itemsRemaining--;
                    if (itemsRemaining == 0)
                    {
                        break;
                    }
                    text +=
                        xmlNode.SelectSingleNode("title").InnerText + "\r\n\t" +
                        xmlNode.SelectSingleNode("link").InnerText + "\r\n";
                }

                chatClient.PostMessage(e.Channel, text);
            }

            return null;
        }

        public bool ShouldExecute()
        {
            return Status == ItemStatus.Active;
        }
    }
}
