using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.Models;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerUploadImage : SetUp
{
    //[Test]
    //public async Task UploadImage_ShouldReturnOkResultWithImageModifyResponseGetSingleResponse_WhenSuccessful()
    //{
    //    UserService.PostImage(Arg.Any<PostImageRequest>()).Returns(Task.FromResult(new GetSingleResponse<ModifyResponse<Image>>()));
    //    S3Service.SendFileToS3Bucket(Arg.Any<IFormFile>()).Returns(Task.FromResult(new GetSingleResponse<string>()));

    //    var response = await Controller.UploadImage(File) as NoContentResult;

    //    Assert.NotNull(response);
    //    Assert.That(response!.StatusCode, Is.EqualTo(204));
    //}

    //[Test]
    //public void UploadImage_ShouldThrowException_WhenExceptionThrownInUserService()
    //{
    //    UserService.PostImage(Arg.Any<PostImageRequest>()).Returns(Task.FromResult(new GetSingleResponse<ModifyResponse<Image>>
    //    {
    //        Entity = null,
    //        Error = new Exception(),
    //        TimeStamp = DateTime.Now
    //    }));
    //    S3Service.SendFileToS3Bucket(Arg.Any<IFormFile>()).Returns(Task.FromResult(new GetSingleResponse<string>()));

    //    Assert.ThrowsAsync<Exception>(async () => await Controller.UploadImage(File));
    //}

    //[Test]
    //public async Task UploadImage_ShouldReturnOkResultWithImageModifyResponseGetSingleResponse_WhenExceptionThrownInS3Service()
    //{
    //    UserService.PostImage(Arg.Any<PostImageRequest>()).Returns(Task.FromResult(new GetSingleResponse<ModifyResponse<Image>>()));
    //    S3Service.SendFileToS3Bucket(Arg.Any<IFormFile>()).Returns(Task.FromResult(new GetSingleResponse<string>
    //    {
    //        Entity = null,
    //        Error = new Exception(),
    //        TimeStamp = DateTime.Now
    //    }));

    //    var response = await Controller.UploadImage(File) as NoContentResult;

    //    Assert.Multiple(() =>
    //    {
    //        Assert.NotNull(response);
    //        Assert.That(response!.StatusCode, Is.EqualTo(204));
    //    });
        
    //}
}