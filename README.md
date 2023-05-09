## Azure Communication Service Email output binding for Azure Function

This [Azure Functions custom output binding](https://learn.microsoft.com/en-us/azure/azure-functions/functions-triggers-bindings?tabs=csharp) send Email using  [Azure Communication Service](https://learn.microsoft.com/en-us/azure/communication-services/concepts/email/email-overview)

This project is experimental and not released as nuget package.

## Usage

### App Settings

Setup connectionSting and senderAddress from your Communication Service resource.

```
"AzureWebJobsACSEmailConnecionString": "<YourConnectionString>",
"AzureWebJobsACSEmailSenderAddress": "<YourSenderAddress>",
```

See also
[Quickstart - Create and manage Email Communication Service resource in Azure Communication Service - An Azure Communication Services quickstart | Microsoft Learn](https://learn.microsoft.com/en-us/azure/communication-services/quickstarts/email/create-email-communication-resource)


Use `ACSEmail` attribute.

## Sample Code

#### .NET

```
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
```
-[Basic example]()
-[Async example]()
-[Blob trigger example]()

#### JavaScript

[Sample code]()

- function.json
```function.json
{
  "bindings": [
    {
      "type": "ACSEmail",
      "direction": "out",
      "name": "mail",
      "ConnectionString":"AzureWebJobsACSEmailConnecionString"
    }
    ...
}
```

- application code
```index.js
module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    const name = (req.query.name || (req.body && req.body.name));
    const responseMessage = name
        ? "Hello, " + name + ". This HTTP triggered function executed successfully."
        : "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.";

    context.bindings.mail = {
        recipientToAddresses : [process.env.testAddress1],
        subject : "Test from function",
        plainTextContent : "Hi from JSFunc",
    };
    context.res = {
        // status: 200, /* Defaults to 200 */
        body: responseMessage
    };
}
```

## Known issue

- extensions.json for non .NET language worker
  https://github.com/Azure/azure-functions-core-tools/issues/3361
- 0-byte attchment file
  https://github.com/Azure/azure-sdk-for-net/issues/36086

## Reference for custom binding

https://github.com/Azure/azure-webjobs-sdk/wiki/Creating-custom-input-and-output-bindings
