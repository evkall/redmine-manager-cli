using System;
using System.Collections.Specialized;

using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;


namespace RedmineManagerCLI.ManagementService
{
    public class RedmineIssueCategory : ICreateable, IReadable, IDeleteable, IUpdateable
    {
        private readonly ILogger<Management> log;
        public RedmineIssueCategory(ILogger<Management> log)
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
            log.LogInformation("Read issue category");
            var issueCategory = manager.GetObject<IssueCategory>(id, null);
            Console.WriteLine($"Категория задач:\n\tID: {issueCategory.Id}\n\tНазвание: {issueCategory.Name}\n\t");
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
            // manager.DeleteObject<IssueCategory>(id);
        }

        public void GetList(RedmineManager manager, NameValueCollection parametrs)
        {
            log.LogInformation("Get list issue category");
            var issueCaregories = manager.GetObjects<IssueCategory>(parametrs);
            
            Console.WriteLine("Категории задач:\n");
            foreach (var issueCaregory in issueCaregories)
            {
                Console.WriteLine($"\tID: {issueCaregory.Id}\n\tНазвание: {issueCaregory.Name}\n\t");
            }
        }
    }
}