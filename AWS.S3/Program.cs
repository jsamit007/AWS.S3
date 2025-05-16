using Amazon.S3;
using Amazon.S3.Model;
using AWS.Cognito;
using AWS.EventBridge;
using AWS.S3.Service;
using AWS.ServiceProvider;
using AWS.SNS.Service;
using AWS.SQS.Service;
using AWS.StepFunction;
using Carbon.Storage;
using System.Net.Mime;
using System.Text.Json;

var client = new ServiceProvider().GetS3Service();
var s3Service = new S3Service(client);

var bucketName = "com.order.jas";
var fileName = "test.mp4";
var contentType = new ContentType { Name = "video/mp4" };

var uploadId = await s3Service.InitiateMultipartUpload(bucketName, fileName, contentType);

var filedata = File.ReadAllBytes(fileName);

var partSize = 5 * 1024 * 1024; // 5 MB
var partCount = (int)Math.Ceiling((double)filedata.Length / partSize);
var partsEtag = new List<PartETag>();
for (int i = 1; i <= partCount; i++)
{
    var partUrl = await s3Service.GetUploadMultipartUrl(bucketName, fileName, contentType, uploadId.UploadId, i);
    var partData = filedata.Skip((i - 1) * partSize).Take(partSize).ToArray();
    var responseFrom = await UploadFileThroughUrl(partUrl, partData, contentType);
    partsEtag.Add(new PartETag(i,$"file-upload-{i}"));
}

var response = await s3Service.CompleteMultipartUpload(bucketName, fileName, uploadId.UploadId);
//var url = await s3Service.GetDownloadPreSignedUrl(bucketName, fileName, contentType);
var outputFile = @"C:\Users\PC\OneDrive\Documents\testfiles\test.mp4";
var byteResponse = s3Service.GetObjectMultiPart(bucketName, fileName, partSize, outputFile);

static async Task<HttpResponseMessage> UploadFileThroughUrl(string url,byte[] data, ContentType contentType) 
{
    using HttpClient httpClient = new HttpClient();

    var requestContent = new ByteArrayContent(data);
    requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType.Name);

    var response = await httpClient.PutAsync(url,requestContent);
    return response;
}

Console.WriteLine();

//Console.WriteLine(JsonSerializer.Serialize(s3Service.PutBucketAsync("js-amit-007"), new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(s3Service.ListBucketsAsync(), new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(s3Service.PutBucketAsync("js-amit-007"), new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(s3Service.UploadFileAsync("js-amit-007","test1.xml","test.xml"), new JsonSerializerOptions { WriteIndented = true }));
//var file = s3Service.GetObjectAsync("js-amit-007", "test.xml").Result;
//Console.WriteLine(JsonSerializer.Serialize(s3Service.DeleteBucketAsync("js-amit-007"), new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(file, new JsonSerializerOptions { WriteIndented = true }));

// SNS Test
//var snsClient = new S3ServiceProvider().GetSNSService();
//var snsService = new SNSService(snsClient);

//Console.WriteLine(JsonSerializer.Serialize(snsService.CreateTopicAsync("js-amit-topic"), new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(snsService.AddSubscriberAsync("js-amit-topic","lambda", "arn:aws:lambda:eu-north-1:480238144354:function:sns-message"), new JsonSerializerOptions { WriteIndented = true }));
//var topicArn = snsService.ListTopicsAsync().Result.Topics.FirstOrDefault(x => x.TopicArn.Contains("js-amit-topic"))?.TopicArn;
//Console.WriteLine(JsonSerializer.Serialize(snsService.PublishMessageAsync(topicArn,"Hello From Progarm.cs"), new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(snsService.UnsubscribeAll(topicArn!), new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(snsService.DeleteTopicAsync(topicArn!), new JsonSerializerOptions { WriteIndented = true }));

// SQS Test
//var sqsClient = new ServiceProvider().GetSQSService();
//var sqsService = new SQSService(sqsClient);

//Console.WriteLine(JsonSerializer.Serialize(sqsService.CreateQueueAsync("js-amit-queue"), new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(sqsService.ListQueuesResponseAsync(), new JsonSerializerOptions { WriteIndented = true }));
/*for(int i=0; i<5; i++)
    Console.WriteLine(
    JsonSerializer.Serialize(
        sqsService.SendMessageAsync("https://sqs.eu-north-1.amazonaws.com/480238144354/js-amit-queue",
        JsonSerializer.Serialize(new { Id = Guid.NewGuid()})
        ), new JsonSerializerOptions { WriteIndented = true }));*/

//Console.WriteLine(JsonSerializer.Serialize(sqsService.ReceiveMessageAsync(
//    "https://sqs.eu-north-1.amazonaws.com/480238144354/js-amit-queue",true), 
//    new JsonSerializerOptions { WriteIndented = true }
//    ));

//Console.WriteLine(JsonSerializer.Serialize(sqsService.DeleteQueueResponseAsync("js-amit-queue"), new JsonSerializerOptions { WriteIndented = true }));

// Event Bridge
//var eventBridge = new ServiceProvider().GetEventBridgeService();
//var ebService = new EventBridgeService(eventBridge);

//Console.WriteLine(JsonSerializer.Serialize(ebService.PutRuleAsync("app-test-rule"),new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(ebService.PutTargetsAsync("app-test-rule", "arn:aws:lambda:eu-north-1:480238144354:function:test-function"),new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(ebService.PutEventsAsync(),new JsonSerializerOptions { WriteIndented = true }));

/*var cognitoClient = new ServiceProvider().GetAmazonCognitoIdentityProviderService();
var cognitoService = new CognitoService(cognitoClient);

var result = await cognitoService.AdminInitiateAuthResponseAsync("eu-north-1_oOzCqNMNo", "525ctgo4dfv5j48sv3e8m9mtrj", "708c89fc-2041-70fe-7cd4-dbf9de977221","A123456s@");

var IdToken = result.AuthenticationResult.IdToken;
var AccessToken = result.AuthenticationResult.AccessToken;
Console.WriteLine(AccessToken);*/

//var stepFunctionClient = new ServiceProvider().GetAmazonStepFunctionsClient();
//var stepFunctionService = new StepFunctionService(stepFunctionClient);
//var executionResult = await stepFunctionService.StartExecutionAsync(
//          "arn:aws:states:eu-north-1:480238144354:stateMachine:TestMachine1",
//        JsonSerializer.Serialize(new { OrderType = "Refund" })
//  );

/*Console.WriteLine(JsonSerializer.Serialize(
        stepFunctionService.DescribeExecutionAsync(
            ""
        ),
        new JsonSerializerOptions { WriteIndented = true })
    );*/