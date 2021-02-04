using System;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.CommandLine.Invocation;
using System.Collections.Generic;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Redmine.Net.Api;

using RedmineManagerCLI.ConnectionService;


namespace RedmineManagerCLI
{
    class Program
    {
        static async Task Main(string[] args) => await BuildCommandLine()
            .UseHost(_ => Host.CreateDefaultBuilder(),
                host =>
                {
                    host.ConfigureAppConfiguration((configuration) =>
                    {
                        configuration.Sources.Clear();
                        configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); //TODO: Add environment configuration provider
                    });
                    host.ConfigureLogging((hostContext, logging) =>
                    {
                        Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(hostContext.Configuration)
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .CreateLogger();

                    });
                    host.ConfigureServices(services =>
                    {
                        services.AddSingleton<IConnection<RedmineManager>, Connection>();
                        services.AddSingleton<IRedmineIssue, RedmineIssue>(); //TODO: Add IConnection<RedmineManager> to RedmineIssue constructor
                    });
                    host.UseSerilog();
                })
            .UseDefaults()
            .Build()
            .InvokeAsync(args);

        private static CommandLineBuilder BuildCommandLine()
        {
            var idOption = new Option<string>(
                "--id",
                description: "Redmine object id.");
            idOption.IsRequired = true;

            var readRedmineIssueCommand = new Command("read"){
                Handler = CommandHandler.Create<IHost, string>(ReadRedmineIssue)
            };
            readRedmineIssueCommand.AddOption(idOption);

            var issueCommand = new Command("issue")
            {
                Description = "Redmine issue."
            };
            issueCommand.AddCommand(readRedmineIssueCommand);

            var rootCommand = new RootCommand("Redmine Manager CLI.")
            {
                new Option<bool>(
                    "--info",
                    description: "Information about the connected user.")
            };
            rootCommand.Handler = CommandHandler.Create<IHost, bool>(Run);
            rootCommand.AddCommand(issueCommand);

            return new CommandLineBuilder(rootCommand);
        }
        private static RedmineManager Connection(IHost host)
        {
            var connection = host.Services.GetRequiredService<IConnection<RedmineManager>>();
            
            var manager = connection.Connect();
            
            return manager;
        }

        private static void Run(IHost host, bool info)
        {
            if (info)
            {            
                RedmineManager manager;
                try
                {
                    manager = Connection(host);
                }
                catch (ConnectionServiceBaseException)
                {
                    System.Console.WriteLine("Ошибка при подключении к серверу.");
                    return;
                }

                var currentUser = manager.GetCurrentUser();
                Console.WriteLine($"Текущий пользователь:\n\tID: {currentUser.Id}\n\tПолное имя: {currentUser.FirstName} {currentUser.LastName}");
            }
        }

        private static void ReadRedmineIssue(IHost host, string id)
        {
            RedmineManager manager;
            try
            {
                manager = Connection(host);
            }
            catch (ConnectionServiceBaseException)
            {
                System.Console.WriteLine("Ошибка при подключении к серверу.");
                return;
            }

            var serviceProvider = host.Services;
            var redmineIssue = serviceProvider.GetRequiredService<IRedmineIssue>();

            redmineIssue.Read(manager, id);
        }
    }
}
