using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.DAL.RequestsAndResponses;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerChangePassword : SetUp
{
    [Test]
    public async Task ChangePassword_ShouldReturnOkResultWithIdentityResultGetSingleResponse_WhenSuccessful()
    {
        UserService.ChangePasswordAsync(Arg.Any<PasswordChange>()).Returns(Task.FromResult(new GetSingleResponse<IdentityResult>()));

        var response = await Controller.ChangePassword(new PasswordChange()) as OkObjectResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(200));
        Assert.That(response.Value, Is.TypeOf<GetSingleResponse<IdentityResult>>());
    }

    [Test]
    public void ChangePassword_ShouldThrowException_WhenExceptionThrownInService()
    {
        UserService.ChangePasswordAsync(Arg.Any<PasswordChange>()).Returns(Task.FromResult(new GetSingleResponse<IdentityResult>
        {
            Entity = null,
            Error = new Exception(),
            TimeStamp = DateTime.Now
        }));

        Assert.ThrowsAsync<Exception>(async () => await Controller.ChangePassword(new PasswordChange()));
    }
}