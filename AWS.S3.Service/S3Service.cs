
using Amazon.S3;
using Amazon.S3.Model;

namespace AWS.S3.Service;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;

    public S3Service(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<ListBucketsResponse> ListBucketsAsync()
    {
        return await _s3Client.ListBucketsAsync();
    }

    public async Task<PutBucketResponse> PutBucketAsync(string bucketName)
    {
        return await _s3Client.PutBucketAsync(new PutBucketRequest
        {
            BucketName = bucketName,
            UseClientRegion = true
        });
    }

    public async Task<PutObjectResponse> UploadFileAsync(string bucketName, string keyName, string filePath)
    {
        return await _s3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest
        {
            BucketName = bucketName,
            Key = keyName,
            FilePath = filePath,
            ContentType = "text/xml"
        });
    }

    public async Task<DeleteBucketResponse> DeleteBucketAsync(string bucketName)
    {
        var listOfObjects = await ListAllObjectsAsync(bucketName);
        if(listOfObjects != null && listOfObjects.S3Objects.Count > 0)
        {
            foreach (var obj in listOfObjects.S3Objects)
                await DeleteObjectAsync(bucketName, obj.Key);

            return await _s3Client.DeleteBucketAsync(new DeleteBucketRequest
            {
                BucketName = bucketName
            });
        }
        return default!;
    }

    public async Task<ListObjectsV2Response> ListAllObjectsAsync(string bucketName)
    {
        return await _s3Client.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = bucketName
        });
    }

    public async Task<DeleteObjectResponse> DeleteObjectAsync(string bucketName,string fileName)
    {
        return await _s3Client.DeleteObjectAsync(bucketName,key: fileName);
    }

    public async Task<GetObjectResponse> GetObjectAsync(string bucketName, string fileName)
    {
        return await _s3Client.GetObjectAsync(bucketName, fileName);
    }
}

