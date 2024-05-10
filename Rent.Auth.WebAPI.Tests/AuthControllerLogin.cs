using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.Auth.DAL.CustomExceptions;
using Rent.Auth.DAL.RequestsAndResponses;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerLogin : SetUp
{
    [Test]
    public async Task Login_ShouldReturnOkResultWithAuthTokenGetSingleResponse_WhenCorrectlyAuthorized()
    {
        Service.LoginAsync(Arg.Any<SignInUser>()).Returns(Task.FromResult(new GetSingleResponse<AuthToken>()));

        var response = await Controller.Login(new SignInUser()) as OkObjectResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(200));
        Assert.That(response.Value, Is.TypeOf<GetSingleResponse<AuthToken>>());
    }

    [Test]
    public void Login_ShouldThrowException_WhenExceptionThrownInService()
    {
        Service.LoginAsync(Arg.Any<SignInUser>()).Returns(Task.FromResult(new GetSingleResponse<AuthToken>
        {
            Entity = null,
            Error = new Exception(),
            TimeStamp = DateTime.Now
        }));

        Assert.ThrowsAsync<Exception>(async () => await Controller.Login(new SignInUser()));
    }

}