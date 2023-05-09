
using System.IO;
using Azure;
using Azure.Communication.Email;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;


namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail.Client
{
    internal class ACSEmailClient : IACSEmailClient
    {
        private EmailClient _client;
        private ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;

        public ACSEmailClient(string connectionString, ILoggerFactory loggerFactory)
        {
            _client = new EmailClient(connectionString);
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger(LogCategories.Bindings);
        }

        public async Task<EmailSendOperation> SendACSEmailAsync(ACSEmailContext msg, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                EmailMessage emailMessage = new EmailMessage(
                msg.SenderAddress,
                 new EmailRecipients(
                     msg.RecipientToAddresses.ToList().ConvertAll(
                        new Converter<String, EmailAddress>(StringToEmailAddress)),
                     msg.RecipientCCAddresses.ToList().Count > 0 ? msg.RecipientCCAddresses.ToList().ConvertAll(
                        new Converter<String, EmailAddress>(StringToEmailAddress)) : Enumerable.Empty<EmailAddress>(),
                     msg.RecipientBCCAddresses.ToList().Count > 0 ? msg.RecipientBCCAddresses.ToList().ConvertAll(
                        new Converter<String, EmailAddress>(StringToEmailAddress)) : Enumerable.Empty<EmailAddress>()
                 ),
                new EmailContent(msg.Subject)
                {
                    PlainText = msg.PlainTextContent,
                    Html = msg.HtmlContent
                }
                );
                if (msg.AttachmentFilePath != null)
                {
                    emailMessage.Attachments.Add(
                        new EmailAttachment(msg.AttachmentName, msg.AttachmentContentType, new BinaryData(File.ReadAllBytes(msg.AttachmentFilePath))));
                }
                if (msg.AttachmentFromStream != null)
                {
                    emailMessage.Attachments.Add(
                        new EmailAttachment(msg.AttachmentName, msg.AttachmentContentType, BinaryData.FromStream(msg.AttachmentFromStream)));
                }
                EmailSendOperation emailSendOperation = await _client.SendAsync(wait: WaitUntil.Completed, emailMessage, cancellationToken);
                string operationId = emailSendOperation.Id;
                _logger.LogInformation($"Email sent operation finished: OperationId = {operationId}, Status = {emailSendOperation.Value.Status}");
                return emailSendOperation;
            } 
            catch (RequestFailedException ex)
                {
                    /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                    _logger.LogError($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
                    throw ex;
                }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw ex;

            }
        }
        public static EmailAddress StringToEmailAddress(String s)
        {
            return new EmailAddress(s);
        }
    }
}