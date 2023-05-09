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
using System.Collections;
using System.Collections.Generic;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [ACSEmail] out ACSEmailContext mail,
            ILogger log)
        {

            string address1 = Environment.GetEnvironmentVariable("testAddress1");
            string address2 = Environment.GetEnvironmentVariable("testAddress2");
            string address3 = Environment.GetEnvironmentVariable("testAddress3");
            string address4 = Environment.GetEnvironmentVariable("testAddress4");

            mail = new ACSEmailContext();
            mail.RecipientToAddresses = new List<string>() { address1, address2 };
            mail.RecipientCCAddresses = new List<string>() { address3 };
            mail.RecipientBCCAddresses = new List<string>() { address4 };
            mail.Subject = "Test acs email from function"; 
            mail.PlainTextContent = "Hello world";
            mail.HtmlContent = "<html><body>This is the html body</body></html>";

            mail.AttachmentFilePath = $"{Directory.GetCurrentDirectory()}\\host.json";
            mail.AttachmentName = "host.json";
            mail.AttachmentContentType = "application/json";
            return (ActionResult) new OkObjectResult("OK");
        }
    }
}
