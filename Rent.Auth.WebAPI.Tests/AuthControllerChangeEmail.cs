using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.Auth.DAL.RequestsAndResponses;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerChangeEmail : SetUp
{
    [Test]
    public async Task ChangeEmail_ShouldReturnOkResultWithIdentityResultGetSingleResponse_WhenSuccessful()
    {
        Service.ChangeEmailAsync(Arg.Any<EmailChange>()).Returns(Task.FromResult(new GetSingleResponse<IdentityResult>()));

        var response = await Controller.ChangeEmail(new EmailChange()) as OkObjectResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(200));
        Assert.That(response.Value, Is.TypeOf<GetSingleResponse<IdentityResult>>());
    }

    [Test]
    public void ChangeEmail_ShouldThrowException_WhenExceptionThrownInService()
    {
        Service.ChangeEmailAsync(Arg.Any<EmailChange>()).Returns(Task.FromResult(new GetSingleResponse<IdentityResult>
        {
            Entity = null,
            Error = new Exception(),
            TimeStamp = DateTime.Now
        }));

        Assert.ThrowsAsync<Exception>(async () => await Controller.ChangeEmail(new EmailChange()));
    }
}