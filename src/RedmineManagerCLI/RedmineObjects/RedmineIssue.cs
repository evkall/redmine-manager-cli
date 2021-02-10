using System;
using System.Collections.Specialized;

using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Newtonsoft.Json;

using RedmineManagerCLI.RedmineObjects;

namespace RedmineManagerCLI
{
    public class RedmineIssue : ICreateable, IReadable, IDeleteable, IUpdateable
    {
        public void Create(RedmineManager manager, JsonTextReader reader)
        {
            // Issue templateIssue = new();
            // templateIssue.ReadJson(reader);
            
            // var issue = manager.CreateObject<Issue>(templateIssue);
            // System.Console.WriteLine(issue.Id);
        }
        public void Read(RedmineManager manager, string id)
        {
            // var issue = manager.GetObject<Issue>(id, null);
            // System.Console.WriteLine(issue.Id);
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
            // var issues = manager.GetObjects<Issue>(parametrs);
            // foreach (var issue in issues)
            // {
            //     Console.WriteLine(issue.Id);
            // }
        }
    }
}