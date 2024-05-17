using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.DAL.RequestsAndResponses;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerSignUp : SetUp
{
    [Test]
    public async Task SignUp_ShouldReturnOkResultWithIdentityResultGetSingleResponse_WhenNoExceptionThrownIsService()
    {
        UserService.SignUpAsync(Arg.Any<SignUpUser>()).Returns(Task.FromResult(new GetSingleResponse<IdentityResult>
        {
            Entity = new IdentityResult(),
            Error = null,
            TimeStamp = DateTime.Now
        }));

        var response = await Controller.SignUp(new SignUpUser()) as NoContentResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(204));
    }

    [Test]
    public void SignUp_ShouldThrowException_WhenExceptionThrownInService()
    {
        UserService.SignUpAsync(Arg.Any<SignUpUser>()).Returns(Task.FromResult(new GetSingleResponse<IdentityResult>
        {
            Entity = null,
            Error = new Exception(),
            TimeStamp = DateTime.Now
        }));

        Assert.ThrowsAsync<Exception>( async () => await Controller.SignUp(new SignUpUser()));
    }

}