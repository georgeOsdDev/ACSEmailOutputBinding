using Microsoft.Azure.WebJobs.Extensions.ACSEmail.Bindings;
using Microsoft.Azure.WebJobs.Extensions.ACSEmail.Config;
using Microsoft.Azure.WebJobs.Extensions.ACSEmail.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail
{
    /// <summary>
    /// Extension methods for  ACSEmail integration
    /// </summary>
    public static class ACSEmailWebJobsBuilderExtensions
    {
        /// <summary>
        /// Adds the  ACSEmail extension to the provided <see cref="IWebJobsBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IWebJobsBuilder"/> to configure.</param>
        public static IWebJobsBuilder AddACSEmail(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<ACSEmailExtensionConfigProvider>()
                .ConfigureOptions<ACSEmailOptions>((rootConfig, extensionPath, options) =>
                {
                    // Set the default, which can be overridden.
                    options.ConnectionString = rootConfig[ACSEmailExtensionConfigProvider.AzureWebJobsACSEmailConnecionString];

                    // Set the default, which can be overridden.
                    options.SenderAddress = rootConfig[ACSEmailExtensionConfigProvider.AzureWebJobsACSEmailSenderAddress];

                    IConfigurationSection section = rootConfig.GetSection(extensionPath);
                    ACSEmailHelpers.ApplyConfiguration(section, options);
                });

            builder.Services.AddSingleton<IACSEmailClientFactory, ACSEmailClientFactory>();

            return builder;
        }

        public static IWebJobsBuilder AddACSEmail(this IWebJobsBuilder builder, Action<ACSEmailOptions> configure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddACSEmail();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}