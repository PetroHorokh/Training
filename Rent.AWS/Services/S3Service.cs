using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Rent.AWS.S3.Services.Contracts;
using Rent.ResponseAndRequestLibrary;

namespace Rent.AWS.S3.Services;

/// <summary>
/// Service for working with S3
/// </summary>
/// <param name="config"></param>
public class S3Service(IConfiguration config) : IS3Service
{
    /// <summary>
    /// Upload specified file to S3 specific bucket
    /// </summary>
    /// <param name="file">Parameter with file to upload file</param>
    /// <returns></returns>
    public async Task<Response<string>> SendFileToS3Bucket(IFormFile file)
    {
        var result = new Response<string>();

        try
        {
            var bucketName = config["AWS:ImageBucket"]!;
            var region = config["AWS:Region"]!;
            var keyName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.GetTempFileName();

            await using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var credentials = new BasicAWSCredentials(config["AWS:AccessKey"], config["AWS:SecreteKey"]);

            var s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.EUNorth1);

            var fileTransferUtility = new TransferUtility(s3Client);
            result.TimeStamp = DateTime.Now;
            await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
            result.Body = $"https://{bucketName}.s3.{region}.amazonaws.com/{keyName}";
        }
        catch (AmazonS3Exception ex)
        {
            result.Exceptions.Add(new Exception("An error occured while uploading file to bucket", ex));
        }

        return result;
    }
}