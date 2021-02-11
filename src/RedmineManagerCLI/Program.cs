using System;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.CommandLine.Invocation;

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

    public enum CreateableRedmineObjects
    {
        RedmineIssue
    }

    public enum UpdateableRedmineObjects
    {
        RedmineIssue
    }

    public enum DeleteableRedmineObjects
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
                        configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                        configuration.AddEnvironmentVariables();
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
                        services.AddSingleton<IManagement, Management>(); //TODO: Add IConnection<RedmineManager> to Management constructor
                    });
                    host.UseSerilog();
                })
            .UseDefaults()
            .Build()
            .InvokeAsync(args);

        private static CommandLineBuilder BuildCommandLine()
        {
            var createCommandArgument = new Argument<CreateableRedmineObjects>(
                "name",
                description: "Redmine object name"
            );

            var readCommandArgument = new Argument<ReadableRedmineObjects>(
                "name",
                description: "Redmine object name"
            );

            var updateCommandArgument = new Argument<UpdateableRedmineObjects>(
                "name",
                description: "Redmine object name"
            );

            var deleteCommandArgument = new Argument<DeleteableRedmineObjects>(
                "name",
                description: "Redmine object name"
            );

            var idOption = new Option<string>(
                "--id",
                description: "Redmine object id");
            idOption.IsRequired = true;

            var parametersSection = new Option<string>(
                "--params-section",
                description: "Create parameters section"
            );
            parametersSection.IsRequired = true;
            parametersSection.AddAlias("-ps");

            var createCommand = new Command("create"){
                Handler = CommandHandler.Create<IHost, CreateableRedmineObjects, string, string>(CreateRedmineObject),
                Description = "Create redmine object"
            };
            createCommand.AddOption(parametersSection);
            createCommand.AddArgument(createCommandArgument);

            var readCommand = new Command("read"){
                Handler = CommandHandler.Create<IHost, ReadableRedmineObjects, string, string>(ReadRedmineObject),
                Description = "Read redmine object"
            };
            readCommand.AddArgument(readCommandArgument);
            readCommand.AddOption(idOption);

            var updateCommand = new Command("update"){
                Handler = CommandHandler.Create<IHost, UpdateableRedmineObjects, string, string, string>(UpdateRedmineObject),
                Description = "Update redmine object"
            };
            updateCommand.AddOption(idOption);
            updateCommand.AddOption(parametersSection);
            updateCommand.AddArgument(updateCommandArgument);

            var deleteCommand = new Command("delete"){
                Handler = CommandHandler.Create<IHost, DeleteableRedmineObjects, string, string>(DeleteRedmineObject),
                Description = "Delete redmine object"
            };
            deleteCommand.AddOption(idOption);
            deleteCommand.AddArgument(updateCommandArgument);
            
            var listParametersSection = new Option<string>(
                "--list-section",
                getDefaultValue: () => "Default",
                description: "List parameters section"
            );
            listParametersSection.AddAlias("-ls");
            
            var listCommand = new Command("list"){
                Handler = CommandHandler.Create<IHost, ReadableRedmineObjects, string, string>(ReadRedmineObjects),
                Description = "Read redmine object list"
            };
            listCommand.AddArgument(readCommandArgument);
            listCommand.AddOption(listParametersSection);

            var connectionSettingsSection = new Option<string>(
                "--connection-section",
                getDefaultValue: () => "Default",
                description: "Connection settings section"
            );
            connectionSettingsSection.AddAlias("-cs");
            
            var rootCommand = new RootCommand("Redmine Manager CLI")
            {
                new Option<bool>(
                    "--info",
                    description: "Information about the connected user")
            };
            rootCommand.Handler = CommandHandler.Create<IHost, bool, string>(Run);
            rootCommand.AddGlobalOption(connectionSettingsSection);
            rootCommand.AddCommand(createCommand);
            rootCommand.AddCommand(readCommand);
            rootCommand.AddCommand(updateCommand);
            rootCommand.AddCommand(deleteCommand);
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

        private static void CreateRedmineObject(IHost host, CreateableRedmineObjects name, string connectionSection, string createSection)
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

            management.CreateRedmineObject(manager, name.ToString(), createSection);
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

        private static void UpdateRedmineObject(IHost host, UpdateableRedmineObjects name, string connectionSection, string id, string updateSection)
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

            management.UpdateRedmineObject(manager, name.ToString(), id, updateSection);
        }

        private static void DeleteRedmineObject(IHost host, DeleteableRedmineObjects name, string connectionSection, string id)
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

            management.DeleteRedmineObject(manager, name.ToString(), id);
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
