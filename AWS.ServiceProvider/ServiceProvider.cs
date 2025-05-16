using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.EventBridge;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.StepFunctions;

namespace AWS.ServiceProvider;

public class ServiceProvider 
{
    public IAmazonS3 GetS3Service(bool isLocal=true)
    {
        if (!isLocal)
            return new AmazonS3Client(RegionEndpoint.EUNorth1);

        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY")!;
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECURITY_KEY")!;
        var region = Amazon.RegionEndpoint.EUNorth1;
        return new AmazonS3Client(accessKey,secretKey,RegionEndpoint.EUNorth1);
    }

    public IAmazonSimpleNotificationService GetSNSService(bool isLocal = true)
    {
        if (!isLocal)
            return new AmazonSimpleNotificationServiceClient(RegionEndpoint.EUNorth1);

        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY")!;
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECURITY_KEY")!;
        var region = Amazon.RegionEndpoint.EUNorth1;
        return new AmazonSimpleNotificationServiceClient(accessKey, secretKey, RegionEndpoint.EUNorth1);
    }

    public IAmazonSQS GetSQSService(bool isLocal = true)
    {
        if (!isLocal)
            return new AmazonSQSClient(RegionEndpoint.EUNorth1);

        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY")!;
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECURITY_KEY")!;
        var region = Amazon.RegionEndpoint.EUNorth1;
        return new AmazonSQSClient(accessKey, secretKey, RegionEndpoint.EUNorth1);
    }

    public IAmazonEventBridge GetEventBridgeService(bool isLocal = true)
    {
        if (!isLocal)
            return new AmazonEventBridgeClient(RegionEndpoint.EUNorth1);

        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY")!;
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECURITY_KEY")!;
        var region = Amazon.RegionEndpoint.EUNorth1;
        return new AmazonEventBridgeClient(accessKey, secretKey, RegionEndpoint.EUNorth1);
    }

    public IAmazonCognitoIdentityProvider GetAmazonCognitoIdentityProviderService(bool isLocal = true)
    {
        if (!isLocal)
            return new AmazonCognitoIdentityProviderClient(RegionEndpoint.EUNorth1);

        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY")!;
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECURITY_KEY")!;
        var region = Amazon.RegionEndpoint.EUNorth1;
        return new AmazonCognitoIdentityProviderClient(accessKey, secretKey, RegionEndpoint.EUNorth1);
    }

    public IAmazonStepFunctions GetAmazonStepFunctionsClient(bool isLocal = true)
    {
        if(!isLocal)
            return new AmazonStepFunctionsClient(RegionEndpoint.EUNorth1);
        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY")!;
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECURITY_KEY")!;
        var region = Amazon.RegionEndpoint.EUNorth1;
        return new AmazonStepFunctionsClient(accessKey, secretKey, RegionEndpoint.EUNorth1);
    }
}