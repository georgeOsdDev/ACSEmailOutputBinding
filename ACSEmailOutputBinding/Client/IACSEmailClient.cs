using Azure.Communication.Email;

namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail.Client
{
    internal interface IACSEmailClient
    {
        Task<EmailSendOperation> SendACSEmailAsync(ACSEmailContext email, CancellationToken cancellationToken = default(CancellationToken));
    }
}