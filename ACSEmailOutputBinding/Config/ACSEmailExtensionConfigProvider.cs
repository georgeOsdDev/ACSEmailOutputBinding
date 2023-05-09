using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Options;
using Microsoft.Azure.WebJobs.Extensions.ACSEmail.Client;
using Microsoft.Azure.WebJobs.Extensions.ACSEmail.Bindings;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail.Config
{
    /// <summary>
    /// Defines the configuration options for the ACSEmail binding.
    /// </summary>
    [Extension("ACSEmail")]
    internal class ACSEmailExtensionConfigProvider : IExtensionConfigProvider
    {
        internal const string AzureWebJobsACSEmailConnecionString = "AzureWebJobsACSEmailConnecionString";
        internal const string AzureWebJobsACSEmailSenderAddress = "AzureWebJobsACSEmailSenderAddress";

        private readonly IOptions<ACSEmailOptions> _options;
        private ConcurrentDictionary<string, IACSEmailClient> _ACSEmailClientCache = new ConcurrentDictionary<string, IACSEmailClient>();
        private ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ACSEmailExtensionConfigProvider(IOptions<ACSEmailOptions> options, IACSEmailClientFactory clientFactory, ILoggerFactory loggerFactory)
        {
            _options = options;
            ClientFactory = clientFactory;
            _loggerFactory = loggerFactory;
        }

        internal IACSEmailClientFactory ClientFactory { get; set; }

        /// <inheritdoc />
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _logger = _loggerFactory.CreateLogger(LogCategories.Bindings);

            context
                .AddConverter<JObject, ACSEmailContext>(ACSEmailHelpers.CreateMessage);

            var rule = context.AddBindingRule<ACSEmailAttribute>();
            rule.AddValidator(ValidateBinding);
            rule.BindToCollector<ACSEmailContext>(attr => CreateCollector(attr));
        }

        private IAsyncCollector<ACSEmailContext> CreateCollector(ACSEmailAttribute attr)
        {
            string connectionString = FirstOrDefault(attr.ConnectionString, _options.Value.ConnectionString);
            IACSEmailClient client = _ACSEmailClientCache.GetOrAdd(connectionString, a => ClientFactory.Create(a, _loggerFactory));
            return new ACSEmailAsyncCollector(_options.Value, attr, client);
        }

        private void ValidateBinding(ACSEmailAttribute attribute, Type type)
        {
            ValidateBinding(attribute);
        }

        private void ValidateBinding(ACSEmailAttribute attribute)
        {
            string connectionString = FirstOrDefault(attribute.ConnectionString, _options.Value.ConnectionString);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "The ACSEmail ConnectionString must be set");
            }
        }

        private static string FirstOrDefault(params string[] values)
        {
            return values.FirstOrDefault(v => !string.IsNullOrEmpty(v));
        }
    }
}
