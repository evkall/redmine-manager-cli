using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Redmine.Net.Api;
using Redmine.Net.Api.Types;

using RedmineManagerCLI.RedmineObjects;

namespace RedmineManagerCLI
{
    public class RedmineIssue : IReadable
    {
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