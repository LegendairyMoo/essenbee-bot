﻿using System.Collections.Generic;
using System.Linq;
using Essenbee.Bot.Core.Interfaces;
using Essenbee.Bot.Core.Messaging;
using Hangfire;
using Serilog;

namespace Essenbee.Bot.Web
{
    public class HangfireActionScheduler : IActionScheduler
    {
        public IList<IChatClient> ChatClients { get; set; }

        public HangfireActionScheduler()
        {
        }

        public HangfireActionScheduler(IList<IChatClient> chatClients)
        {
            ChatClients = chatClients;
        }

        public void Schedule(IScheduledAction action)
        {
            Log.Information($"Scheduling {action.Name} with Hangfire server...");

            switch (action)
            {
               case DelayedMessage delayedMsg:
                    var msg = delayedMsg.Message;
                    var chnl = delayedMsg.Channel;

                    foreach (var chatClient in ChatClients)
                    {
                        if (!string.IsNullOrWhiteSpace(chnl))
                        {
                            BackgroundJob.Schedule(() => chatClient.PostMessage(chnl, msg), delayedMsg.Delay);
                        }
                        else
                        {
                            BackgroundJob.Schedule(() => chatClient.PostMessage(msg), delayedMsg.Delay);
                        }
                    }
                    break;

                case RepeatingMessage repeatingMsg:
                    var message = repeatingMsg.Message;
                    var channel = repeatingMsg.Channel;

                    foreach (var chatClient in ChatClients)
                    {
                        if (!string.IsNullOrWhiteSpace(channel))
                        {
                            RecurringJob.AddOrUpdate(
                            repeatingMsg.Name,
                            () => chatClient.PostMessage(channel, message),
                            Cron.MinuteInterval(repeatingMsg.IntervalInMinutes));
                        }
                        else
                        {
                            RecurringJob.AddOrUpdate(
                            repeatingMsg.Name,
                            () => chatClient.PostMessage(message),
                            Cron.MinuteInterval(repeatingMsg.IntervalInMinutes));
                        }
                    }
                    break;
            }
        }

        public List<string> GetRunningJobs<T>()
        {
            var jobs = GetRunningHangfireJobs().Where(o => o.Value.Job.Type == typeof(T));
            return jobs.Select(j => j.Key).ToList();
        }

        public List<string> GetRunningJobs()
        {
            var jobs = GetRunningHangfireJobs();
            return jobs.Select(j => j.Key).ToList();
        }

        public List<string> GetScheduledJobs()
        {
            var jobs = GetScheduledHangfireJobs();
            return jobs.Select(j => j.Key).ToList();
        }

        public List<string> GetEnqueuedJobs()
        {
            var jobs = GetEnqueuedHangfireJobs();
            return jobs.Select(j => j.Key).ToList();
        }

        private List<KeyValuePair<string, Hangfire.Storage.Monitoring.ProcessingJobDto>> GetRunningHangfireJobs()
        {
            return JobStorage.Current.GetMonitoringApi()
                .ProcessingJobs(0, int.MaxValue).ToList();
        }

        private List<KeyValuePair<string, Hangfire.Storage.Monitoring.ScheduledJobDto>> GetScheduledHangfireJobs()
        {
            return JobStorage.Current.GetMonitoringApi()
                .ScheduledJobs(0, int.MaxValue).ToList();
        }

        private List<KeyValuePair<string, Hangfire.Storage.Monitoring.EnqueuedJobDto>> GetEnqueuedHangfireJobs(string queue = "default")
        {
            return JobStorage.Current.GetMonitoringApi()
                .EnqueuedJobs(queue, 0, int.MaxValue).ToList();
        }
    }
}
