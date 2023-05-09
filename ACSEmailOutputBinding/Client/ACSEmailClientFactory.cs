using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail.Client
{
    internal class ACSEmailClientFactory : IACSEmailClientFactory
    {
        public IACSEmailClient Create(string connectionString, ILoggerFactory loggerFactory)
        {
            return new ACSEmailClient(connectionString, loggerFactory);
        }
    }
}