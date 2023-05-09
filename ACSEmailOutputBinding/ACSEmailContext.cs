using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail
{
    public class ACSEmailContext
    {
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        [JsonProperty("senderAddress")]
        public string SenderAddress { get; set; }

        [JsonProperty("recipientToAddresses")]
        public IList<string> RecipientToAddresses { get; set; }

        [JsonProperty("recipientCCAddresses")]
        public IList<string> RecipientCCAddresses { get; set; }

        [JsonProperty("recipientBCCAddresses")]
        public IList<string> RecipientBCCAddresses { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("htmlContent")]
        public string HtmlContent { get; set; }

        [JsonProperty("plainTextContent")]
        public string PlainTextContent { get; set; }

        [JsonProperty("attachmentFilePath")]
        public string AttachmentFilePath { get; set; }

        public Stream AttachmentFromStream { get; set; }

        [JsonProperty("attachmentName")]
        public string AttachmentName { get; set; }

        [JsonProperty("attachmentContentType")]
        public string AttachmentContentType { get; set; }

    }
}
