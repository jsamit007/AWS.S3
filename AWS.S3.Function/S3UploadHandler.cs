
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using AWS.S3.Service;
using AWS.ServiceProvider;
using DynamoDB.Service;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace AWS.S3.Function;

internal class S3UploadHandler
{
    public async Task Handle(S3Event s3Event, ILambdaContext context)
    {
        Console.WriteLine(JsonSerializer.Serialize(s3Event));
        var bucketName = s3Event.Records[0].S3.Bucket.Name;
        var objectKey = s3Event.Records[0].S3.Object.Key;

        var client = new S3ServiceProvider().GetS3Service(false);
        var s3Service = new S3Service(client);

        var getObjectResponse = await s3Service.GetObjectAsync(bucketName, objectKey);

        if(getObjectResponse != null)
        {
            var objectMetaData = new ObjectMetaData
            {
                ETag = getObjectResponse.ETag,
                BucketName = bucketName,
                Key = objectKey,
                LastModified = getObjectResponse.LastModified
            };

            var dynamoDbClient = DynamoDB.Service.ServiceProvider.GetDynamoDbClient(false);
            await new DBContext<ObjectMetaData>(dynamoDbClient).AddProductAsync(objectMetaData);
        }
    }
}
