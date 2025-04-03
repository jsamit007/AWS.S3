using Amazon.S3;

namespace AWS.ServiceProvider;

public class S3ServiceProvider 
{
    public IAmazonS3 GetService()
    {
        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY")!;
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECURITY_KEY")!;
        var region = Amazon.RegionEndpoint.EUNorth1;
        return new AmazonS3Client(accessKey,secretKey,Amazon.RegionEndpoint.EUNorth1);
    }
}