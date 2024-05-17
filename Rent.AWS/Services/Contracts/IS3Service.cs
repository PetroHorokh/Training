using Microsoft.AspNetCore.Http;
using Rent.DAL.RequestsAndResponses;

namespace Rent.AWS.S3.Services.Contracts;

public interface IS3Service
{
    Task<GetSingleResponse<string>> SendFileToS3Bucket(IFormFile file);
}