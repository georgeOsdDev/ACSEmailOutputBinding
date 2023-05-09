using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.ACSEmail;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class Function3
    {
        [FunctionName("Function3")]
        public void Run([BlobTrigger("samples-workitems/{name}")] Stream myBlob,
            string name,
            [ACSEmail()] out ACSEmailContext mail,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            string address1 = Environment.GetEnvironmentVariable("testAddress1");

            mail = new ACSEmailContext();
            mail.RecipientToAddresses = new List<string>() { address1 };
            mail.Subject = "Test from function3";
            mail.PlainTextContent = "Func3";
            mail.AttachmentFromStream = myBlob;
            mail.AttachmentName = name;
            mail.AttachmentContentType = "text/html";
        }
    }
}
