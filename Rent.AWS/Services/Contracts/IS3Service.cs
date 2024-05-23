using Microsoft.AspNetCore.Http;
using Rent.ResponseAndRequestLibrary;

namespace Rent.AWS.S3.Services.Contracts;

public interface IS3Service
{
    Task<Response<string>> SendFileToS3Bucket(IFormFile file);
}