using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace AWS.SNS.Service;

public class SNSService
{
    private readonly IAmazonSimpleNotificationService _snsClient;

    public SNSService(IAmazonSimpleNotificationService snsClient)
    {
        _snsClient = snsClient;
    }

    public async Task<CreateTopicResponse> CreateTopicAsync(string topicName)
    {
        var createTopicRequest = new CreateTopicRequest
        {
            Name = topicName
        };

        var response = await _snsClient.CreateTopicAsync(createTopicRequest);
        return response;
    }

    public async Task<SubscribeResponse> AddSubscriberAsync(string topicName,string type,string endpoint)
    {
        var topics = await ListTopicsAsync();
        var topicArn = topics.Topics.FirstOrDefault(x => x.TopicArn.Contains(topicName))?.TopicArn;
        var subscribeRequest = new SubscribeRequest
        {
            TopicArn = topicArn,
            Protocol = type, // or "sms", "http", "https", etc.
            ReturnSubscriptionArn = true,
            Endpoint = endpoint
        };

        var response = await _snsClient.SubscribeAsync(subscribeRequest);
        return response;
    }

    public async Task<ListTopicsResponse> ListTopicsAsync()
    {
        return await _snsClient.ListTopicsAsync(new ListTopicsRequest());
    }

    public async Task<PublishResponse> PublishMessageAsync(string topicArn,string Message)
    {
        var request = new PublishRequest
        {
            TopicArn = topicArn,
            Message = Message
        };
        return await _snsClient.PublishAsync(request);
    }

    public async Task<UnsubscribeResponse> UnsubscribeAsync(string subscriptionArn)
    {
        var unsubscribeRequest = new UnsubscribeRequest
        {
            SubscriptionArn = subscriptionArn
        };

        return await _snsClient.UnsubscribeAsync(unsubscribeRequest);
    }

    public async Task<IEnumerable<Subscription>> ListSubscriptionsResponseAsync(string topicArn)
    {
        var paginator = _snsClient.Paginators.ListSubscriptionsByTopic(new ListSubscriptionsByTopicRequest { TopicArn = topicArn });

        var subscriptions = new List<Subscription>();

        await foreach (var page in paginator.Responses)
            subscriptions.AddRange(page.Subscriptions);

        return subscriptions;
    }

    public async Task<List<UnsubscribeResponse>> UnsubscribeAll(string topicName)
    {
        var subscriptions = await ListSubscriptionsResponseAsync(topicName);
        var unsubscribeTasks = subscriptions.Select(subscription => UnsubscribeAsync(subscription.SubscriptionArn));
        var result = await Task.WhenAll(unsubscribeTasks);
        return result.ToList();
    }

    public async Task<DeleteTopicResponse> DeleteTopicAsync(string topicArn)
    {
        var deleteTopicRequest = new DeleteTopicRequest
        {
            TopicArn = topicArn
        };

        return await _snsClient.DeleteTopicAsync(deleteTopicRequest);
    }
}
