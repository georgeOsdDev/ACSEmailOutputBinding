using System.Collections.Concurrent;
using Microsoft.Azure.WebJobs.Extensions.ACSEmail.Client;

namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail.Bindings
{
    internal class ACSEmailAsyncCollector : IAsyncCollector<ACSEmailContext>
    {
        private readonly ACSEmailOptions _options;
        private readonly ACSEmailAttribute _attribute;
        private readonly ConcurrentQueue<ACSEmailContext> _messages = new ConcurrentQueue<ACSEmailContext>();
        private readonly IACSEmailClient _client;

        public ACSEmailAsyncCollector(ACSEmailOptions options, ACSEmailAttribute attribute, IACSEmailClient client)
        {
            _options = options;
            _attribute = attribute;
            _client = client;
        }

        public Task AddAsync(ACSEmailContext item, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (item == null)
            {
                throw new ArgumentNullException("No mail item was provided.");
            }

            ACSEmailHelpers.DefaultMessageProperties(item, _options, _attribute);

            if (item.SenderAddress == null || string.IsNullOrEmpty(item.SenderAddress))
            {
                throw new InvalidOperationException("A 'SenderAddress' must be specified for the message.");
            }

            _messages.Enqueue(item);

            return Task.CompletedTask;
        }

        public async Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            while (_messages.TryDequeue(out ACSEmailContext message))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _client.SendACSEmailAsync(message, cancellationToken);
            }
        }
    }
}