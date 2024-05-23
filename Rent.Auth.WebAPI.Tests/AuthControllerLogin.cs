using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.ResponseAndRequestLibrary;


namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerLogin : SetUp
{
    [Test]
    public async Task Login_ShouldReturnOkResultWithAuthTokenGetSingleResponse_WhenCorrectlyAuthorized()
    {
        UserService.LoginAsync(Arg.Any<SignInUser>()).Returns(Task.FromResult(new Response<AuthToken>()));

        var response = await Controller.Login(new SignInUser()) as OkObjectResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(200));
        Assert.That(response.Value, Is.TypeOf<Response<AuthToken>>());
    }

    [Test]
    public async Task Login_ShouldThrowException_WhenExceptionThrownInService()
    {
        UserService.LoginAsync(Arg.Any<SignInUser>()).Returns(Task.FromResult(new Response<AuthToken>
        {
            Exceptions = [new(), new()]
        }));

        var result = await Controller.Login(new SignInUser()) as ObjectResult;

        Assert.NotNull(result);
        Assert.That(result!.StatusCode, Is.EqualTo(500));
        Assert.That(result!.Value, Is.Not.Empty);
        Assert.That(result.Value as List<Exception>, Has.Count.EqualTo(2));
    }

}