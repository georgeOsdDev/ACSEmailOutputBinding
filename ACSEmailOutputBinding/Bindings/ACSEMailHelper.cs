using System.Net.Mail;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.ACSEmail.Bindings
{
    internal class ACSEmailHelpers
    {
        internal static String Apply(String current, string value)
        {
            if (TryParseAddress(value, out EmailAddress mail))
            {
                return mail.Address;
            }
            return current;
        }

        internal static bool TryParseAddress(string value, out EmailAddress email)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            try
            {
                // MailAddress will auto-parse the name from a string like "testuser@test.com <Test User>"
                MailAddress mailAddress = new MailAddress(value);
                string displayName = string.IsNullOrEmpty(mailAddress.DisplayName) ? null : mailAddress.DisplayName;
                email = new EmailAddress(mailAddress.Address, displayName);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        internal static void DefaultMessageProperties(ACSEmailContext mail, ACSEmailOptions options, ACSEmailAttribute attribute)
        {
            // Apply message defaulting
            if (mail.SenderAddress == null)
            {
                if (!string.IsNullOrEmpty(attribute.SenderAddress))
                {
                    if (!TryParseAddress(attribute.SenderAddress, out EmailAddress from))
                    {
                        throw new ArgumentException("Invalid SenderAddress specified");
                    }
                    mail.SenderAddress = from.Address;
                }
                else if (options.SenderAddress != null)
                {
                    mail.SenderAddress = options.SenderAddress;
                }
            }
            if (mail.RecipientToAddresses == null)
            {
                mail.RecipientToAddresses = new List<string>();
            }
            if (mail.RecipientCCAddresses == null)
            {
                mail.RecipientCCAddresses = new List<string>();
            }
            if (mail.RecipientBCCAddresses == null)
            {
                mail.RecipientBCCAddresses = new List<string>();
            }
        }

        internal static ACSEmailContext CreateMessage(string input)
        {
            JObject json = JObject.Parse(input);
            return CreateMessage(json);
        }

        internal static ACSEmailContext CreateMessage(JObject input)
        {
            return input.ToObject<ACSEmailContext>();
        }


        internal static void ApplyConfiguration(IConfiguration config, ACSEmailOptions options)
        {
            if (config == null)
            {
                return;
            }

            options.ConnectionString = config.GetValue<string>("connectionString");
            string senderAddress = config.GetValue<string>("senderAddress");
            options.SenderAddress = ACSEmailHelpers.Apply(options.SenderAddress, senderAddress);

            config.Bind(options);
        }
    }
}