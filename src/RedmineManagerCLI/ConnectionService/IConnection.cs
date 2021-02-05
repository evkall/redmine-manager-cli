using Redmine.Net.Api;


namespace RedmineManagerCLI.ConnectionService
{
    interface IConnection<T> where T: RedmineManager
    {
        T Connect(string section);
    }
}