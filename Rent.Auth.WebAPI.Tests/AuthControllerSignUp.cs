using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.Auth.DAL.CustomExceptions;
using Rent.Auth.DAL.RequestsAndResponses;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerSignUp : SetUp
{
    [Test]
    public async Task SignUp_ShouldReturnOkResultWithIdentityResultGetSingleResponse_WhenNoExceptionThrownIsService()
    {
        Service.SignUpAsync(Arg.Any<SignUpUser>()).Returns(Task.FromResult(new GetSingleResponse<IdentityResult>
        {
            Entity = new IdentityResult(),
            Error = null,
            TimeStamp = DateTime.Now
        }));

        var response = await Controller.SignUp(new SignUpUser()) as OkObjectResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(200));
        Assert.That(response.Value, Is.TypeOf<GetSingleResponse<IdentityResult>>());
    }

    [Test]
    public void SignUp_ShouldThrowException_WhenExceptionThrownInService()
    {
        Service.SignUpAsync(Arg.Any<SignUpUser>()).Returns(Task.FromResult(new GetSingleResponse<IdentityResult>
        {
            Entity = null,
            Error = new Exception(),
            TimeStamp = DateTime.Now
        }));

        Assert.ThrowsAsync<Exception>( async () => await Controller.SignUp(new SignUpUser()));
    }

}