using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RedmineManagerCLI
{
    public class ManagementService : IManagement
    {
        private readonly ILogger<ConnectionService> log;

        public ManagementService(ILogger<ConnectionService> log)
        {
            this.log = log;
        }
        void Create()
        {
            log.LogInformation("Created");
        }
        void Read()
        {
            log.LogInformation("Readed");
        }
        void Update()
        {
            log.LogInformation("Updated");
        }
        void Delete()
        {
            log.LogInformation("Deleted");
        }

    }
}