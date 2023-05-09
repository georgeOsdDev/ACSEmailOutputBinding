using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.ACSEmail;
using Azure.Communication.Email;
using System.Text;
using System.Collections.Generic;

namespace FunctionApp1
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [ACSEmail(ConnectionString = "MyCustomACSEmailConnecionStringKeyName")] IAsyncCollector<ACSEmailContext> messageCollector,
                        ILogger log)
        {

            string address1 = Environment.GetEnvironmentVariable("testAddress1");

            var mail = new ACSEmailContext();
            mail.RecipientToAddresses = new List<string>() { address1 };
            mail.Subject = "Test from function2";
            mail.PlainTextContent = "Func2";
            await messageCollector.AddAsync(mail);
        }
    }
}
