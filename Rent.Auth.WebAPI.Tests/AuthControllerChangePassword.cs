using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.ResponseAndRequestLibrary;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerChangePassword : SetUp
{
    [Test]
    public async Task ChangePassword_ShouldReturnOkResultWithIdentityResultGetSingleResponse_WhenSuccessful()
    {
        UserService.ChangePasswordAsync(Arg.Any<PasswordChange>()).Returns(Task.FromResult(new Response<IdentityResult>()));

        var response = await Controller.ChangePassword(new PasswordChange()) as OkObjectResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(200));
        Assert.That(response.Value, Is.TypeOf<Response<IdentityResult>>());
    }

    [Test]
    public async Task ChangePassword_ShouldThrowException_WhenExceptionThrownInService()
    {
        UserService.ChangePasswordAsync(Arg.Any<PasswordChange>()).Returns(Task.FromResult(new Response<IdentityResult>
        {
            Exceptions = [new(), new()]
        }));

        var result = await Controller.ChangePassword(new PasswordChange()) as ObjectResult;

        Assert.NotNull(result);
        Assert.That(result!.StatusCode, Is.EqualTo(500));
        Assert.That(result!.Value, Is.Not.Empty);
        Assert.That(result.Value as List<Exception>, Has.Count.EqualTo(2));
    }
}