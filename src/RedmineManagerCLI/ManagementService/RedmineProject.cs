using System;
using System.Collections.Specialized;

using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;


namespace RedmineManagerCLI.ManagementService
{
    public class RedmineProject : ICreateable, IReadable, IDeleteable, IUpdateable
    {
        private readonly ILogger<Management> log;
        public RedmineProject(ILogger<Management> log)
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
            log.LogInformation("Read project");
            var project = manager.GetObject<Project>(id, null);
            Console.WriteLine($"Проект:\n\tID: {project.Id}\n\tНазвание: {project.Name}\n\t");
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
            // manager.DeleteObject<Project>(id);
        }

        public void GetList(RedmineManager manager, NameValueCollection parametrs)
        {
            log.LogInformation("Get list project");
            var projects = manager.GetObjects<Project>(parametrs);

            Console.WriteLine("Проекты:\n");
            foreach (var project in projects)
            {
                Console.WriteLine($"\tID: {project.Id}\n\tНазвание: {project.Name}\n\t");
            }
        }
    }
}