using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace AWS.SQS.Function;

internal class SQSMessageHandler
{
    public void Handle(SQSEvent sqsEvent, ILambdaContext context)
    {
        Console.WriteLine(JsonSerializer.Serialize(sqsEvent.Records));
    }
}
