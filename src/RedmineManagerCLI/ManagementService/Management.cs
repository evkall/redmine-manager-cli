using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Redmine.Net.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RedmineManagerCLI.RedmineObjects;


namespace RedmineManagerCLI.ManagementService
{
    
    public class Management : IManagement
    {
        private readonly List<Type> CreateableRedmineObjects = new(){typeof(RedmineIssue)};
        private readonly List<Type> ReadableRedmineObjects = new(){typeof(RedmineIssue)};
        private readonly List<Type> UpdateableRedmineObjects = new(){typeof(RedmineIssue)};
        private readonly List<Type> DeleteableRedmineObjects = new(){typeof(RedmineIssue)};
        private readonly ILogger<Management> log;
        private readonly IConfiguration config;

        public Management(ILogger<Management> log, IConfiguration config)
        {
            this.log = log;
            this.config = config;
        }

        public void CreateRedmineObject(RedmineManager manager, string name, string section)
        {
            var redmineObjectJson = JObject.Parse(System.IO.File.ReadAllText("appsettings.json")).GetValue("RedmineObjectParameters").SelectToken(section).ToString();
            JsonTextReader redmineObjectReader = new(new StringReader(redmineObjectJson));

            var CreateableRedmineObjectType = from CreateableRedmineObject in CreateableRedmineObjects
                                            where CreateableRedmineObject.Name == name
                                            select CreateableRedmineObject;

            var createableRedmineObject = (ICreateable)Activator.CreateInstance(CreateableRedmineObjectType.Single());
            createableRedmineObject.Create(manager, redmineObjectReader);
        }
        
        public void ReadRedmineObject(RedmineManager manager, string name, string id)
        {
            var readableRedmineObjectType = from ReadableRedmineObject in ReadableRedmineObjects
                                            where ReadableRedmineObject.Name == name
                                            select ReadableRedmineObject;
            
            var readableRedmineObject = (IReadable)Activator.CreateInstance(readableRedmineObjectType.Single());
            readableRedmineObject.Read(manager, id);
        }
        public void UpdateRedmineObject(RedmineManager manager, string name, string id, string section)
        {
            var redmineObjectJson = JObject.Parse(System.IO.File.ReadAllText("appsettings.json")).GetValue("RedmineObjectParameters").SelectToken(section).ToString();
            JsonTextReader redmineObjectReader = new(new StringReader(redmineObjectJson));

            var UpdateableRedmineObjectType = from UpdateableRedmineObject in UpdateableRedmineObjects
                                              where UpdateableRedmineObject.Name == name
                                              select UpdateableRedmineObject;

            var updateableRedmineObject = (IUpdateable)Activator.CreateInstance(UpdateableRedmineObjectType.Single());
            updateableRedmineObject.Update(manager, id, redmineObjectReader);
        }
        public void DeleteRedmineObject(RedmineManager manager, string name, string id)
        {
            var deleteableRedmineObjectType = from DeleteableRedmineObject in DeleteableRedmineObjects
                                            where DeleteableRedmineObject.Name == name
                                            select DeleteableRedmineObject;
            
            var deleteableRedmineObject = (IDeleteable)Activator.CreateInstance(deleteableRedmineObjectType.Single());
            deleteableRedmineObject.Delete(manager, id);
        }
        public void ReadRedmineObjects(RedmineManager manager, string name, string section)
        {
            var parameters = config.GetSection("ListParameters").GetSection(section).GetChildren();

            var filter = new NameValueCollection(parameters.Count());
            foreach (var parameter in parameters)
            {
                filter.Add(parameter.Key, parameter.Value);
            }

            var readableRedmineObjectType = from ReadableRedmineObject in ReadableRedmineObjects
                                            where ReadableRedmineObject.Name == name
                                            select ReadableRedmineObject;
            
            var readableRedmineObject = (IReadable)Activator.CreateInstance(readableRedmineObjectType.Single());
            readableRedmineObject.GetList(manager, filter);

        }
    }
}