using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.ComponentModel;
using System.IO;

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

                        configuration
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
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
                        services.AddSingleton<IConnection, ConnectionService>();
                        services.AddSingleton<IManagement, ManagementService>();
                    });
                    host.UseSerilog();
                })
            .UseDefaults()
            .Build()
            .InvokeAsync(args);

        private static CommandLineBuilder BuildCommandLine()
        {
            var rootCommand = new RootCommand("Redmine Manager CLI."){
                new Option<bool>(
                    "--info",
                    description: "Information about the connected user."),
            };
            rootCommand.Handler = CommandHandler.Create<ConnectionOptions, IHost>(Run);

            return new CommandLineBuilder(rootCommand);
        }

        private static void Run(ConnectionOptions options, IHost host)
        {
            var serviceProvider = host.Services;
            var connection = serviceProvider.GetRequiredService<IConnection>();

            connection.Connect(options);

            // var name = options.Name;
            // logger.LogInformation(GreetEvent, "Greeting was requested for: {name}", name);
            // greeter.Greet(name);
        }
    }

}
