using Amazon.SQS;
using Amazon.SQS.Model;

namespace AWS.SQS.Service;

public class SQSService
{
    private readonly IAmazonSQS _sqsClient;

    public SQSService(IAmazonSQS sqsClient)
    {
        _sqsClient = sqsClient;
    }

    public async Task<CreateQueueResponse> CreateQueueAsync(string queueName)
    {
        return await _sqsClient.CreateQueueAsync(new CreateQueueRequest
        {
            QueueName = queueName,
            Attributes = new Dictionary<string, string>
            {
                { "DelaySeconds", "10" },
                { "MessageRetentionPeriod", "86400" }
            }
        });
    }

    public async Task<ListQueuesResponse> ListQueuesResponseAsync()
    {
        return await _sqsClient.ListQueuesAsync(new ListQueuesRequest());
    }

    public async Task<DeleteQueueResponse> DeleteQueueResponseAsync(string queueUrl)
    {
        return await _sqsClient.DeleteQueueAsync(new DeleteQueueRequest
        {
            QueueUrl = queueUrl
        });
    }

    public async Task<SendMessageResponse> SendMessageAsync(string queueUrl, string messageBody)
    {
        return await _sqsClient.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = messageBody
        });
    }

    public async Task<ReceiveMessageResponse> ReceiveMessageAsync(string queueUrl,bool isReadAfterDelete)
    {

        var messages = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
        {
            QueueUrl = queueUrl,
            MaxNumberOfMessages = 2,
        });

        if (isReadAfterDelete)
        {
            foreach (var message in messages.Messages)
            {
                DeleteMessageAsync(queueUrl, message.ReceiptHandle);
            }
        }

        return messages;
    }

    public async Task<DeleteMessageResponse> DeleteMessageAsync(string queueUrl,string receiptHandle)
    {
        return await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest
        {
            QueueUrl = queueUrl,
            ReceiptHandle = receiptHandle
        });
    }
}
