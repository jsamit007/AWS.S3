
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace DynamoDB.Stream.Function;

internal class DatabaseTriggerHandler
{
    public void Handle(DynamoDBEvent streamEvent, ILambdaContext context)
    {
        Console.WriteLine(JsonSerializer.Serialize(streamEvent.Records));
    }
}
