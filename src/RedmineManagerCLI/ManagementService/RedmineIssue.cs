using System;
using System.Collections.Specialized;

using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;


namespace RedmineManagerCLI.ManagementService
{
    public class RedmineIssue : ICreateable, IReadable, IDeleteable, IUpdateable
    {
        private readonly ILogger<Management> log;
        public RedmineIssue(ILogger<Management> log)
        {
            this.log = log;
        }
        public void Create(RedmineManager manager, JsonTextReader reader)
        {
            // Issue templateIssue = new();
            // templateIssue.ReadJson(reader);
            
            // var issue = manager.CreateObject<Issue>(templateIssue);
            // System.Console.WriteLine(issue.Id);
        }
        public void Read(RedmineManager manager, string id)
        {
            log.LogInformation("Read issue");
            var issue = manager.GetObject<Issue>(id, null);
            Console.WriteLine($"Задача:\n\tID: {issue.Id}\n\tНазвание: {issue.Subject}\n\t");
        }

        public void Update(RedmineManager manager, string id, JsonTextReader reader)
        {
            // Issue templateIssue = new();
            // templateIssue.ReadJson(reader);
            
            // manager.UpdateObject<Issue>(id, templateIssue);

            // var issue = manager.GetObject<Issue>(id, null);
            // System.Console.WriteLine(issue.Id);
        }

        public void Delete(RedmineManager manager, string id) 
        {
            // manager.DeleteObject<Issue>(id);
        }

        public void GetList(RedmineManager manager, NameValueCollection parametrs)
        {
            log.LogInformation("Get list issue");
            var issues = manager.GetObjects<Issue>(parametrs);

            Console.WriteLine("Задачи:\n");
            foreach (var issue in issues)
            {
                Console.WriteLine($"\tID: {issue.Id}\n\tНазвание: {issue.Subject}\n\t");
            }
        }
    }
}