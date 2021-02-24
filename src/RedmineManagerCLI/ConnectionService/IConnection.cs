using Redmine.Net.Api;


namespace RedmineManagerCLI.ConnectionService
{
    public interface IConnection<T> where T: RedmineManager
    {
        T Connect(string section);
    }
}