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
using RedmineManagerCLI.ManagementService;


namespace RedmineManagerCLI
{
    public enum ReadableRedmineObjects
    {
        RedmineIssue
    }
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
                        services.AddSingleton<IManagement, Management>(); //TODO: Add IConnection<RedmineManager> to RedmineIssue constructor
                    });
                    host.UseSerilog();
                })
            .UseDefaults()
            .Build()
            .InvokeAsync(args);

        private static CommandLineBuilder BuildCommandLine()
        {
            var readCommandArgument = new Argument<ReadableRedmineObjects>(
                "name",
                description: "Redmine object name"
            );

            var connectionSettingsSection = new Option<string>(
                "--connection-section",
                getDefaultValue: () => "Default",
                description: "Connection settings section"
            );
            connectionSettingsSection.AddAlias("-cs");

            var listParametersSection = new Option<string>(
                "--list-section",
                getDefaultValue: () => "Default",
                description: "List parameters section"
            );
            listParametersSection.AddAlias("-ls");

            var idOption = new Option<string>(
                "--id",
                description: "Redmine object id");
            idOption.IsRequired = true;

            var readCommand = new Command("read"){
                Handler = CommandHandler.Create<IHost, ReadableRedmineObjects, string, string>(ReadRedmineObject),
                Description = "Read redmine object"
            };
            readCommand.AddArgument(readCommandArgument);
            readCommand.AddOption(idOption);
            
            var listCommand = new Command("list"){
                Handler = CommandHandler.Create<IHost, ReadableRedmineObjects, string, string>(ReadRedmineObjects),
                Description = "Read redmine object list"
            };
            listCommand.AddArgument(readCommandArgument);
            listCommand.AddOption(listParametersSection);

            var rootCommand = new RootCommand("Redmine Manager CLI")
            {
                new Option<bool>(
                    "--info",
                    description: "Information about the connected user")
            };
            rootCommand.Handler = CommandHandler.Create<IHost, bool, string>(Run);
            rootCommand.AddGlobalOption(connectionSettingsSection);
            rootCommand.AddCommand(readCommand);
            rootCommand.AddCommand(listCommand);

            return new CommandLineBuilder(rootCommand);
        }
        private static RedmineManager Connection(IHost host, string section)
        {
            var connection = host.Services.GetRequiredService<IConnection<RedmineManager>>();
            var manager = connection.Connect(section);
            
            return manager;
        }

        private static void Run(IHost host, bool info, string connectionSection)
        {
            if (info)
            {         
                RedmineManager manager;
                try
                {
                    manager = Connection(host, connectionSection);
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

        private static void ReadRedmineObject(IHost host, ReadableRedmineObjects name, string id, string connectionSection)
        {
            RedmineManager manager;
            try
            {
                manager = Connection(host, connectionSection);
            }
            catch (ConnectionServiceBaseException)
            {
                Console.WriteLine("Ошибка при подключении к серверу.");
                return;
            }

            var serviceProvider = host.Services;
            var management = serviceProvider.GetRequiredService<IManagement>();

            management.ReadRedmineObject(manager, name.ToString(), id);
        }

        private static void ReadRedmineObjects(IHost host, ReadableRedmineObjects name, string connectionSection, string listSection)
        {
            RedmineManager manager;
            try
            {
                manager = Connection(host, connectionSection);
            }
            catch (ConnectionServiceBaseException)
            {
                Console.WriteLine("Ошибка при подключении к серверу.");
                return;
            }

            var serviceProvider = host.Services;
            var management = serviceProvider.GetRequiredService<IManagement>();

            management.ReadRedmineObjects(manager, name.ToString(), listSection);
        }
    }
}
