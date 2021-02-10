using Redmine.Net.Api;


namespace RedmineManagerCLI.ManagementService
{
    interface IManagement
    {
        void CreateRedmineObject(RedmineManager manager, string name, string section);
        void ReadRedmineObject(RedmineManager manager, string name, string id);
        void UpdateRedmineObject(RedmineManager manager, string name, string id, string section);
        void DeleteRedmineObject(RedmineManager manager, string name, string id);
        void ReadRedmineObjects(RedmineManager manager, string name, string section);
    }
}