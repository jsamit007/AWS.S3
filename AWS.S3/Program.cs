using AWS.S3.Service;
using AWS.ServiceProvider;
using AWS.SNS.Service;
using AWS.SQS.Service;
using System.Text.Json;

//var client = new S3ServiceProvider().GetS3Service();
//var s3Service = new S3Service(client);
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
var sqsClient = new S3ServiceProvider().GetSQSService();
var sqsService = new SQSService(sqsClient);

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

Console.WriteLine(JsonSerializer.Serialize(sqsService.DeleteQueueResponseAsync("js-amit-queue"), new JsonSerializerOptions { WriteIndented = true }));
