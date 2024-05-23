using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.ResponseAndRequestLibrary;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerRenewAccessToken : SetUp
{
    [Test]
    public async Task RenewAccessToken_ShouldReturnOkResultWithStringGetSingleResponse_WhenRenewed()
    {
        UserService.RenewAccessToken(Arg.Any<AuthToken>()).Returns(Task.FromResult(new Response<string>()));

        var response = await Controller.RenewAccessToken(new AuthToken()) as OkObjectResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(200));
        Assert.That(response.Value, Is.TypeOf<Response<string>>());
    }

    [Test]
    public async Task Login_ShouldThrowException_WhenExceptionThrownInService()
    {
        UserService.RenewAccessToken(Arg.Any<AuthToken>()).Returns(Task.FromResult(new Response<string>
        {
            Exceptions = [new(), new()]
        }));

        var result = await Controller.RenewAccessToken(new AuthToken()) as ObjectResult;

        Assert.NotNull(result);
        Assert.That(result!.StatusCode, Is.EqualTo(500));
        Assert.That(result!.Value, Is.Not.Empty);
        Assert.That(result.Value as List<Exception>, Has.Count.EqualTo(2));
    }
}