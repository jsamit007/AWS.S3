using Amazon.Lambda.Core;
using Amazon.Lambda.KinesisEvents;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace AWS.Kinesis.Function;

internal class KinesisStreamHandler 
{
    public void Handle(KinesisEvent kinesisEvent, ILambdaContext context)
    {
        Console.WriteLine(JsonSerializer.Serialize(kinesisEvent.Records));
        var records = kinesisEvent.Records;
        foreach(var record in records)
        {
            Console.WriteLine($"Received: {Convert.FromBase64String(record.EventSource)}");
        }
    }
}
