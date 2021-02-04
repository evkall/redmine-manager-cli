using System.Collections.Specialized;

using Redmine.Net.Api;


namespace RedmineManagerCLI.Services
{
    interface ICreatable
    {
        void Create();
    }

    interface IReadable
    {
        void Read(RedmineManager manager, string id);

        void GetList(RedmineManager manager, NameValueCollection parameters);
    }

    interface IUpdateable
    {
        void Update(string id);
    }

    interface IDeleteable
    {
        void Delete(string id);
        
    }
}