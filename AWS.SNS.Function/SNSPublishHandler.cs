using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.SimpleNotificationService.Model;
using AWS.ServiceProvider;
using DynamoDB.Service;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace AWS.SNS.Function;

internal class SNSPublishHandler
{
    public async Task Handle(SNSEvent s3Event, ILambdaContext context)
    {
        var message = s3Event.Records[0].Sns.Message;
        var subject = s3Event.Records[0].Sns.Subject;
        var topicArn = s3Event.Records[0].Sns.TopicArn;
        var subscribers = s3Event.Records.Select(x => x.EventSubscriptionArn);

        var messageObject = new SNSMessage
        {
            Message = message,
            Subject = subject,
            TopicArn = topicArn,
            Subscribers = subscribers.ToList()
        };

        var dynamoDbClient = DynamoDB.Service.ServiceProvider.GetDynamoDbClient(false);
        await new DBContext<SNSMessage>(dynamoDbClient).AddProductAsync(messageObject);

       /* var snsClient = new S3ServiceProvider().GetSNSService(false);
        var request = new PublishRequest
        {
            TopicArn = topicArn,
            Message = "Message Received and Saved in SNSMessage Table",
            Subject = "Message Received"
        };
        await snsClient.PublishAsync(request);*/
    }
}
