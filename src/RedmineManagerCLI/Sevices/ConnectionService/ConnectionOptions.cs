namespace RedmineManagerCLI.ConnectionService
{
    public record ConnectionOptions(
        string Host = null, string Login = null, string Password = null, string APIKey = null);
}