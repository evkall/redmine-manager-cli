using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Redmine.Net.Api;
using Redmine.Net.Api.Types;

using RedmineManagerCLI.Services;

namespace RedmineManagerCLI
{
    interface IRedmineIssue : IReadable
    {

    }

    public class RedmineIssue : IRedmineIssue
    {
        private readonly ILogger<RedmineIssue> log;
        private readonly IConfiguration config;

        public RedmineIssue(ILogger<RedmineIssue> log, IConfiguration config)
        {
            this.log = log;
            this.config = config;
        }


        // public void Create() => Manager.CreateObject<Issue>(new Issue());

        public void Read(RedmineManager manager, string id)
        {
            var issue = manager.GetObject<Issue>(id, null);
            System.Console.WriteLine(issue.Id);
        }

        // public void Update(string id) => Manager.UpdateObject<Issue>(id, new Issue());

        // public void Delete(string id) => Manager.DeleteObject<Issue>(id);

        public void GetList(RedmineManager manager, NameValueCollection parametrs)
        {
            var issues = manager.GetObjects<Issue>(parametrs);
        }
    }
}