using Amazon;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace AWS.ServiceProvider;

public class S3ServiceProvider 
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
}