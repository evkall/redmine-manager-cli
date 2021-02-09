using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Newtonsoft.Json;

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

        public void CreateRedmineObject(RedmineManager manager, string name, string section)
        {
            // string json = @"{
            //     'start_date':'2021-02-09',
            //     'project':'2'
            // }";
            
            // var issue = new Issue();
            // string json = @"{
            //     'id': 2
            // }";
            // JsonTextReader reader = new JsonTextReader(new StringReader(json));
            // issue.ReadJson(reader);

            // StringBuilder sb = new StringBuilder();
            // StringWriter sw = new StringWriter(sb);

            // JsonWriter writer = new JsonTextWriter(sw);
            // issue.WriteJson(writer);
            // x.WriteJson(writer);
            // System.Console.WriteLine(sw);
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