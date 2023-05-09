using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail
{
    public class ACSEmailOptions
    {
        public string ConnectionString { get; set; }

        public string SenderAddress { get; set; }
    }
}
