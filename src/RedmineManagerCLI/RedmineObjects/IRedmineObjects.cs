using System.Collections.Specialized;

using Redmine.Net.Api;


namespace RedmineManagerCLI.RedmineObjects
{
    interface ICreatable
    {
        void Create(RedmineManager manager);
    }

    interface IReadable
    {
        void Read(RedmineManager manager, string id);

        void GetList(RedmineManager manager, NameValueCollection parameters);
    }

    interface IUpdateable
    {
        void Update(RedmineManager manager, string id);
    }

    interface IDeleteable
    {
        void Delete(RedmineManager manager, string id);
        
    }
}