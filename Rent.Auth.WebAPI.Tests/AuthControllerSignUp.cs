using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.ResponseAndRequestLibrary;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerSignUp : SetUp
{
    [Test]
    public async Task SignUp_ShouldReturnOkResultWithIdentityResultGetSingleResponse_WhenNoExceptionThrownIsService()
    {
        UserService.SignUpAsync(Arg.Any<SignUpUser>()).Returns(Task.FromResult(new Response<IdentityResult>
        {
            Body = new IdentityResult()
        }));

        var response = await Controller.SignUp(new SignUpUser()) as NoContentResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(204));
    }

    [Test]
    public async Task SignUp_ShouldThrowException_WhenExceptionThrownInService()
    {
        UserService.SignUpAsync(Arg.Any<SignUpUser>()).Returns(Task.FromResult(new Response<IdentityResult>()));

        UserService.SignUpAsync(Arg.Any<SignUpUser>()).Returns(Task.FromResult(new Response<IdentityResult>
        {
            Exceptions = [new(), new()]
        }));

        var result = await Controller.SignUp(new SignUpUser()) as ObjectResult;

        Assert.NotNull(result);
        Assert.That(result!.StatusCode, Is.EqualTo(500));
        Assert.That(result!.Value, Is.Not.Empty);
        Assert.That(result.Value as List<Exception>, Has.Count.EqualTo(2));
    }

}