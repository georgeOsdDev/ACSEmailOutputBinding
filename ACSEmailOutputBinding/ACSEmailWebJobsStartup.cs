using Microsoft.Azure.WebJobs.Extensions.ACSEmail;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(ACSEmailWebJobsStartup))]
namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail
{
    public class ACSEmailWebJobsStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddACSEmail();
        }
    }
}

