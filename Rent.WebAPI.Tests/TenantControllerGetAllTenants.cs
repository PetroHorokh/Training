using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.DAL.DTO;
using Rent.DAL.RequestsAndResponses;
using Rent.Response.Library;

namespace Rent.WebAPI.Tests;

public class TenantControllerGetAllTenants : SetUp
{
    [Test]
    public async Task GetAllTenants_ShouldReturnIEnumerableOfTenantToGedDto_WhenThereAreTenantsPresent()
    {
        Service.GetAllTenantsAsync().Returns(Task.FromResult(new Response<IEnumerable<TenantToGetDto>>()
        {
            Body = new List<TenantToGetDto> { new(), new() }
        }));

        var response = await Controller.GetAllTenants();

        var result = response.Value;

        Assert.NotNull(result);
        Assert.That(result!.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllTenants_ShouldReturnNoContentResult_WhenNoTenantsPresent()
    {
        Service.GetAllTenantsAsync().Returns(Task.FromResult(new Response<IEnumerable<TenantToGetDto>>
        {
            Body = new List<TenantToGetDto>()
        }));

        var response = await Controller.GetAllTenants();

        var result = response.Result as NoContentResult;

        Assert.NotNull(result);
        Assert.That(result!.StatusCode, Is.EqualTo(204));
    }

    [Test]
    public async Task GetAllTenants_ShouldThrowException_WhenExceptionThrownIsService()
    {
        Service.GetAllTenantsAsync().Returns(Task.FromResult(new Response<IEnumerable<TenantToGetDto>>()
        {
            Exceptions = [new()]
        }));

        var response = await Controller.GetAllTenants();

        Assert.NotNull(response.Result);
    }
}