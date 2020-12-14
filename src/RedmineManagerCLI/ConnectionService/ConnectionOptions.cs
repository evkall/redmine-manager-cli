namespace RedmineManagerCLI
{
    public record ConnectionOptions(
        string Host = "", string Login = "", string Password = "", string APIKey = "");
}