using Microsoft.Azure.WebJobs.Description;

namespace Microsoft.Azure.WebJobs
{
    /// <summary>
    /// Attribute used to bind a parameter to a ACSEmail. Message will sent when the
    /// method completes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public sealed class ACSEmailAttribute : Attribute
    {
        /// <summary>
        /// Sets the ConnectionString for the current outgoing message. May include binding parameters.
        /// </summary>
        [AppSetting(Default = "AzureWebJobsACSEmailConnecionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Sets the SenderAddress for the outgoing request. May include binding parameters.
        /// </summary>
        [AutoResolve]
        public string SenderAddress { get; set; }

    }
}
