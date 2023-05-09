using Microsoft.Extensions.Logging;
namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail.Client
{
    internal interface IACSEmailClientFactory
    {
        IACSEmailClient Create(string connectioString, ILoggerFactory loggerFactory);
    }
}
