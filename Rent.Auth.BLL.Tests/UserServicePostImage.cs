using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.Auth.DAL.Models;
using Rent.ResponseAndRequestLibrary;

namespace Rent.Auth.BLL.Tests;

public class UserServicePostImage : SetUp
{
    [Test]
    public async Task PostImage_ShouldReturnNoExceptions_WhenSuccessful()
    {
        UnitOfWork.Images.Add(Arg.Any<Image>()).Returns(new Response<EntityEntry<Image>>()
        {
            Body = null,
            Exceptions = new List<Exception>(),
            TimeStamp = DateTime.Now
        });

        var response = await UserService.PostImage(new PostImageRequest
        {
            Image = File,
            Url = null,
            UserId = Guid.NewGuid()
        });

        Assert.NotNull(response);
        Assert.That(response.Exceptions, Is.Empty);
    }

    [Test]
    public async Task PostImage_ShouldReturnImageModifyResponseGetSingleResponseWithException_WhenExceptionThrownInDALLayer()
    {
        UnitOfWork.Images.Add(Arg.Any<Image>()).Returns(new Response<EntityEntry<Image>>()
        {
            Body = null,
            Exceptions = new List<Exception>(),
            TimeStamp = DateTime.Now
        });

        var response = await UserService.PostImage(new PostImageRequest
        {
            Image = File,
            Url = null,
            UserId = Guid.NewGuid()
        });

        Assert.NotNull(response);
        Assert.Null(response.Body);
        Assert.NotNull(response.Exceptions);
    }
}