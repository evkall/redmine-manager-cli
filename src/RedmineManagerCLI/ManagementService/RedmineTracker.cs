using System;
using System.Collections.Specialized;

using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Microsoft.Extensions.Logging;


namespace RedmineManagerCLI.ManagementService
{
    public class RedmineTracker : IReadable
    {
        private readonly ILogger<Management> log;
        public RedmineTracker(ILogger<Management> log)
        {
            this.log = log;
        }
        public void Read(RedmineManager manager, string id)
        {
            log.LogInformation("Read tracker");
            var tracker = manager.GetObject<Tracker>(id, null);
            Console.WriteLine($"Трекер:\n\tID: {tracker.Id}\n\tНазвание: {tracker.Name}\n\t");
        }

        public void GetList(RedmineManager manager, NameValueCollection parametrs)
        {
            log.LogInformation("Get list tracker");
            var trackers = manager.GetObjects<Tracker>(parametrs);
            
            Console.WriteLine("Трекеры:\n");
            foreach (var tracker in trackers)
            {
                Console.WriteLine($"\tID: {tracker.Id}\n\tНазвание: {tracker.Name}\n\t");
            }
        }
    }
}