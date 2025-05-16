using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using System.Text.Json;

namespace AWS.EventBridge;

public class EventBridgeService
{
    private readonly IAmazonEventBridge _eventBridgeClient;

    public EventBridgeService(IAmazonEventBridge eventBridgeClient)
    {
        _eventBridgeClient = eventBridgeClient;
    }

    public async Task<PutEventsResponse> PutEventsAsync()
    {
        var request = new PutEventsRequest
        {
            Entries = new List<PutEventsRequestEntry>
            {
                new PutEventsRequestEntry
                {
                    Source = "com.mycompany.myapp",
                    DetailType = "Order Submitted",
                    Detail = "{ \"key1\": \"value1\", \"key2\": \"value2\" }",
                    EventBusName = "default"
                }
            }
        };

        Console.WriteLine(JsonSerializer.Serialize(request));
        return await _eventBridgeClient.PutEventsAsync(request);
    }

    public async Task<PutRuleResponse> PutRuleAsync(string ruleName)
    {
        var request = new PutRuleRequest
        {
            Name = ruleName,
            State = RuleState.ENABLED,
            Description = "Rule to trigger Lambda function on S3 events",
            EventBusName = "default",
            RoleArn = "arn:aws:iam::480238144354:role/service-role/Amazon_EventBridge_Invoke_Lambda_1914688184",
            ScheduleExpression = "rate(2 minutes)"
        };
        return await _eventBridgeClient.PutRuleAsync(request);
    }

    public async Task<PutTargetsResponse> PutTargetsAsync(string ruleName, string targetArn)
    {
        var request = new PutTargetsRequest
        {
            Rule = ruleName,
            Targets = new List<Target>
            {
                new Target
                {
                    Id = "1",
                    Arn = targetArn,
                    Input = "{ \"key1\": \"value1\", \"key2\": \"value2\" }"
                }
            },
            EventBusName = "default",
        };
        return await _eventBridgeClient.PutTargetsAsync(request);
    }
}
