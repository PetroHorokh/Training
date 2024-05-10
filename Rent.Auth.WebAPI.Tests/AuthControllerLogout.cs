using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Rent.Auth.WebAPI.Tests;

public class AuthControllerLogout : SetUp
{
    [Test]
    public async Task Logout_ShouldReturnNoContentResult_WhenSuccessful()
    {
        Service.Logout().Returns(Task.CompletedTask);

        var response = await Controller.Logout() as NoContentResult;

        Assert.NotNull(response);
        Assert.That(response!.StatusCode, Is.EqualTo(204));
    }
}