using AWS.S3.Service;
using AWS.ServiceProvider;
using System.Text.Json;

var client = new S3ServiceProvider().GetService();
var s3Service = new S3Service(client);
//Console.WriteLine(JsonSerializer.Serialize(s3Service.ListBucketsAsync(), new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(s3Service.PutBucketAsync("js-amit-007"), new JsonSerializerOptions { WriteIndented = true }));
//Console.WriteLine(JsonSerializer.Serialize(s3Service.UploadFileAsync("js-amit-007","test.xml","test.xml"), new JsonSerializerOptions { WriteIndented = true }));
Console.WriteLine(JsonSerializer.Serialize(s3Service.DeleteBucketAsync("js-amit-007"), new JsonSerializerOptions { WriteIndented = true }));
