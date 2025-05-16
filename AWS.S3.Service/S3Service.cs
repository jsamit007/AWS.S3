
using Amazon.S3;
using Amazon.S3.Model;
using System.Net.Mime;
using CompleteMultipartUploadRequest = Amazon.S3.Model.CompleteMultipartUploadRequest;
using GetObjectRequest = Amazon.S3.Model.GetObjectRequest;
using InitiateMultipartUploadRequest = Amazon.S3.Model.InitiateMultipartUploadRequest;

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
        if (listOfObjects != null && listOfObjects.S3Objects.Count > 0)
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

    public async Task<DeleteObjectResponse> DeleteObjectAsync(string bucketName, string fileName)
    {
        return await _s3Client.DeleteObjectAsync(bucketName, key: fileName);
    }

    public async Task<GetObjectResponse> GetObjectAsync(string bucketName, string fileName)
    {
        return await _s3Client.GetObjectAsync(bucketName, fileName);
    }

    public async Task<string> GetDownloadPreSignedUrl(string bucketName, string key, ContentType contentType)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(5),
            Verb = HttpVerb.GET
        };

        var url = await _s3Client.GetPreSignedURLAsync(request);
        return url;
    }

    public async Task<string> GetUploadPreSignedUrl(string bucketName, string key, ContentType contentType)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(5),
            ContentType = contentType.Name,
            Verb = HttpVerb.PUT
        };

        var url = await _s3Client.GetPreSignedURLAsync(request);
        return url;
    }

    public async Task<string> GetUploadMultipartUrl(string bucketName, string key, ContentType contentType,string uploadId,int partNumber)
    {

        var signedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(5),
            ContentType = contentType.Name,
            Verb = HttpVerb.PUT,
            UploadId = uploadId,
            PartNumber = partNumber
        };

        var url = await _s3Client.GetPreSignedURLAsync(signedUrlRequest);
        return url;
    }

    public async Task<InitiateMultipartUploadResponse> InitiateMultipartUpload(string bucketName, string key, ContentType contentType)
    {
        var initiateRequest = new InitiateMultipartUploadRequest
        {
            BucketName = bucketName,
            Key = key,
            ContentType = contentType.Name
        };

        return await _s3Client.InitiateMultipartUploadAsync(initiateRequest);
    }

    public async Task<CompleteMultipartUploadResponse> CompleteMultipartUpload(string bucketName, string key, string uploadId)
    {
        List<PartETag> partETags = new List<PartETag>();

        foreach (var part in (await ListPartsResponseAsync(bucketName,key,uploadId)).Parts)
        {
            partETags.Add(new PartETag(part.PartNumber, part.ETag));
        } 
        var completeRequest = new CompleteMultipartUploadRequest
        {
            BucketName = bucketName,
            Key = key,
            UploadId = uploadId,
            PartETags = partETags
        };

        return await _s3Client.CompleteMultipartUploadAsync(completeRequest);
    }

    private async Task<ListPartsResponse> ListPartsResponseAsync(string bucketName, string key, string uploadId)
    {
        var request = new ListPartsRequest
        {
            BucketName = bucketName,
            Key = key,
            UploadId = uploadId
        };

        var response = await _s3Client.ListPartsAsync(request);
        return response;
    } 

    public async Task<FileStream> GetObjectMultiPart(string bucketName,string key,int size,string outputFile)
    {
        using var fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None);
        int start = 0;
        int end = size;
        GetObjectResponse response = null!;
        do
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                ByteRange = new ByteRange(start, end)
            };

            response = await _s3Client.GetObjectAsync(request);

            using var responseStream = response.ResponseStream;
            fs.Seek(start, SeekOrigin.Begin);
            await responseStream.CopyToAsync(fs);
            start = end + 1;
            end = start + size - 1;
        }
        while (response != null);
        
        return fs;
    }
}

