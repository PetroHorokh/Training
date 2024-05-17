using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.Auth.DAL.Models;
using Rent.Response.Library;

namespace Rent.Auth.BLL.Tests;

public class UserServicePostImage : SetUp
{
    [Test]
    public async Task PostImage_ShouldReturnImageModifyResponseGetSingleResponseWithoutException_WhenSuccessful()
    {
        UnitOfWork.Images.Add(Arg.Any<Image>()).Returns(new ModifyResponse<Image>
        {
            Status = null,
            Error = null,
            TimeStamp = DateTime.Now
        });

        var response = await UserService.PostImage(new PostImageRequest
        {
            Image = File,
            Url = null,
            UserId = Guid.NewGuid()
        });

        Assert.NotNull(response);
        Assert.NotNull(response.Entity);
        Assert.Null(response.Error);
    }

    [Test]
    public async Task PostImage_ShouldReturnImageModifyResponseGetSingleResponseWithException_WhenExceptionThrownInDALLayer()
    {
        UnitOfWork.Images.Add(Arg.Any<Image>()).Returns(new ModifyResponse<Image>
        {
            Status = null,
            Error = new Exception(),
            TimeStamp = DateTime.Now
        });

        var response = await UserService.PostImage(new PostImageRequest
        {
            Image = File,
            Url = null,
            UserId = Guid.NewGuid()
        });

        Assert.NotNull(response);
        Assert.Null(response.Entity);
        Assert.NotNull(response.Error);
    }
}