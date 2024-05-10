using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.Auth.DAL.AuthModels;
using Rent.Auth.DAL.RequestsAndResponses;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerRenewAccessToken : SetUp
{
    [Test]
    public async Task RenewAccessToken_ShouldReturnOkResultWithStringGetSingleResponse_WhenRenewed()
    {
        Service.RenewAccessToken(Arg.Any<AuthToken>()).Returns(Task.FromResult(new GetSingleResponse<string>()));

        var response = await Controller.RenewAccessToken(new AuthToken()) as OkObjectResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(200));
        Assert.That(response.Value, Is.TypeOf<GetSingleResponse<string>>());
    }

    [Test]
    public void Login_ShouldThrowException_WhenExceptionThrownInService()
    {
        Service.RenewAccessToken(Arg.Any<AuthToken>()).Returns(Task.FromResult(new GetSingleResponse<string>
        {
            Entity = null,
            Error = new Exception(),
            TimeStamp = DateTime.Now
        }));

        Assert.ThrowsAsync<Exception>(async () => await Controller.RenewAccessToken(new AuthToken()));
    }
}