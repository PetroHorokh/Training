using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Rent.DTOs.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.WebAPI.Tests;

public class TenantControllerGetPartialTenants : SetUp
{
    [Test]
    public async Task GetPartialTenants_ShouldReturnTenants_WhenPassedCorrectArgumentsAndTenantsArePresent()
    {
        Service.GetTenantsPartialAsync(Arg.Any<GetPartialRequest>()).Returns(Task.FromResult(new Response<IEnumerable<TenantToGetDto>>()
        {
            Body = [new(), new()]
        }));

        var response = await Controller.GetPartialTenants(0, 2);

        var result = response.Value;

        Assert.NotNull(result);
        Assert.That(result!.Count() == 2);
    }

    [Test]
    public async Task GetPartialTenants_ShouldReturnNoContentResult_WhenNoTenantsPresent()
    {
        Service.GetTenantsPartialAsync(Arg.Any<GetPartialRequest>()).Returns(Task.FromResult(new Response<IEnumerable<TenantToGetDto>>
        {
            Body = new List<TenantToGetDto>()
        }));

        var response = await Controller.GetPartialTenants(1, 1);

        var result = response.Result as NoContentResult;

        Assert.NotNull(result);
        Assert.That(result!.StatusCode, Is.EqualTo(204));
    }

    [Test]
    public void GetPartialTenants_ShouldThrowException_WhenInsufficientArgumentsProvided()
    {
        Service.GetTenantsPartialAsync(Arg.Any<GetPartialRequest>()).Returns(Task.FromResult(new Response<IEnumerable<TenantToGetDto>>()));

        Assert.ThrowsAsync<ArgumentException>(async () => await Controller.GetPartialTenants(-1, -1));
    }

    [Test]
    public void GetPartialTenants_ShouldThrowException_WhenExceptionThrownIsService()
    {
        Service.GetTenantsPartialAsync(Arg.Any<GetPartialRequest>()).ThrowsAsync(new Exception());

        Assert.ThrowsAsync<Exception>(async () => await Controller.GetPartialTenants(1, 1));
    }
}