using System;
using System.Collections.Specialized;

using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;


namespace RedmineManagerCLI.ManagementService
{
    public class RedmineIssueStatus : IReadable
    {
        private readonly ILogger<Management> log;
        public RedmineIssueStatus(ILogger<Management> log)
        {
            this.log = log;
        }

        public void Read(RedmineManager manager, string id)
        {
            log.LogInformation("Read issue status");
            var issueStatus = manager.GetObject<IssueStatus>(id, null);
            Console.WriteLine($"Статус задачи:\n\tID: {issueStatus.Id}\n\tНазвание: {issueStatus.Name}\n\t");
        }

        public void GetList(RedmineManager manager, NameValueCollection parametrs)
        {
            log.LogInformation("Get list issue status");
            var issueStatuses = manager.GetObjects<IssueStatus>(parametrs);
            Console.WriteLine("Статусы задач:\n");
            foreach (var issueStatus in issueStatuses)
            {
                Console.WriteLine($"\tID: {issueStatus.Id}\n\tНазвание: {issueStatus.Name}\n\t");
            }
        }
    }
}