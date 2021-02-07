using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Redmine.Net.Api;

using RedmineManagerCLI.RedmineObjects;


namespace RedmineManagerCLI.ManagementService
{
    public class Management : IManagement
    {
        
        private readonly List<Type> ReadableRedmineObjects = new List<Type>(){typeof(RedmineIssue)};
        private readonly ILogger<Management> log;
        private readonly IConfiguration config;

        public Management(ILogger<Management> log, IConfiguration config)
        {
            this.log = log;
            this.config = config;
        }

        public void CreateRedmineObject(RedmineManager manager, string name)
        {
        }
        
        public void ReadRedmineObject(RedmineManager manager, string name, string id)
        {
            var readableRedmineObjectType = from ReadableRedmineObject in ReadableRedmineObjects
                                            where ReadableRedmineObject.Name == name
                                            select ReadableRedmineObject;
            
            var readableRedmineObject = (IReadable)Activator.CreateInstance(readableRedmineObjectType.Single());
            readableRedmineObject.Read(manager, id);
        }
        public void UpdateRedmineObject(RedmineManager manager, string name){

        }
        public void DeleteRedmineObject(RedmineManager manager, string name){

        }
        public void ReadRedmineObjects(RedmineManager manager, string name, string section)
        {
            var x = config.GetSection("ListParameters").GetSection(section).GetChildren();

            var y = new NameValueCollection(x.Count());

            foreach (var item in x)
            {
                y.Add(item.Key, item.Value);
            }

            var readableRedmineObjectType = from ReadableRedmineObject in ReadableRedmineObjects
                                            where ReadableRedmineObject.Name == name
                                            select ReadableRedmineObject;
            
            var readableRedmineObject = (IReadable)Activator.CreateInstance(readableRedmineObjectType.Single());
            readableRedmineObject.GetList(manager, y);

        }
    }
}