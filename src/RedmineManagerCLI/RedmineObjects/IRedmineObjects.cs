using System.Collections.Specialized;

using Redmine.Net.Api;
using Newtonsoft.Json;


namespace RedmineManagerCLI.RedmineObjects
{
    interface ICreateable
    {
        void Create(RedmineManager manager, JsonTextReader reader);
    }

    interface IReadable
    {
        void Read(RedmineManager manager, string id);

        void GetList(RedmineManager manager, NameValueCollection parameters);
    }

    interface IUpdateable
    {
        void Update(RedmineManager manager, string id, JsonTextReader reader);
    }

    interface IDeleteable
    {
        void Delete(RedmineManager manager, string id);
        
    }
}