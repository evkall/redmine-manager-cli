using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;


namespace RedmineManagerCLI
{
    public class ConnectionService : IConnection
    {
        private readonly ILogger<ConnectionService> log;
        private readonly IConfiguration config;

        // public RedmineManager Manager { get; private set; }

        public ConnectionService(ILogger<ConnectionService> log, IConfiguration config)
        {
            this.log = log;
            this.config = config;
        }
        public void Connect(ConnectionOptions options)
        {
            log.LogInformation(options.ToString());
            log.LogInformation("Connecting...");
        }
    }
}