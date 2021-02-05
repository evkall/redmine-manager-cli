using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Redmine.Net.Api;


namespace RedmineManagerCLI.ConnectionService
{
    public class Connection : IConnection<RedmineManager>
    {
        private readonly ILogger<Connection> log;
        private readonly IConfiguration config;

        public Connection(ILogger<Connection> log, IConfiguration config)
        {
            this.log = log;
            this.config = config;
        }
        public RedmineManager Connect(string section)
        {
            ConnectionOptions options = new ConnectionOptions();
            config.GetSection(nameof(ConnectionOptions)).GetSection(section).Bind(options);

            RedmineManager manager;

            log.LogInformation("Connecting");
            if (String.IsNullOrEmpty(options.Host))
            {
                throw new ConnectionServiceOptionsException("Incorrect host name.", options.ToString());
            }

            if (!String.IsNullOrEmpty(options.APIKey))
            {
                manager = new RedmineManager(options.Host, apiKey: options.APIKey);
            }
            else if (!(String.IsNullOrEmpty(options.Login) || String.IsNullOrEmpty(options.Password)))
            {

                manager = new RedmineManager(options.Host, login: options.Login, password: options.Password);
            }
            else
            {
                throw new ConnectionServiceOptionsException("Incorrect connection options.", options.ToString());
            }

            try
            {
                var userId = manager.GetCurrentUser().Id;
            }
            catch (System.Net.WebException e)
            {

                throw new ConnectionServiceConnectionException("Not logined.", e);
            }

            return manager;
        }
    }
}