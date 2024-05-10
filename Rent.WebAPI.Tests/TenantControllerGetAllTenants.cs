using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rent.DAL.DTO;
using Rent.DAL.RequestsAndResponses;
using Rent.WebAPI.CustomExceptions;

namespace Rent.WebAPI.Tests;

public class TenantControllerGetAllTenants : SetUp
{
    [Test]
    public async Task GetAllTenants_ShouldReturnIEnumerableOfTenantToGedDto_WhenThereAreTenantsPresent()
    {
        Service.GetAllTenantsAsync().Returns(Task.FromResult(new GetMultipleResponse<TenantToGetDto>
            { Collection = new List<TenantToGetDto> { new TenantToGetDto(), new TenantToGetDto() }, Count = 2, Error = null, TimeStamp = DateTime.Now }));

        var response = await Controller.GetAllTenants();

        var result = response.Value;

        Assert.NotNull(result);
        Assert.That(result!.Count() == 2);
    }

    [Test]
    public async Task GetAllTenants_ShouldReturnNoContentResult_WhenNoTenantsPresent()
    {
        Service.GetAllTenantsAsync().Returns(Task.FromResult(new GetMultipleResponse<TenantToGetDto>
            { Collection = null, Count = 0, Error = null, TimeStamp = DateTime.Now }));

        var response = await Controller.GetAllTenants();

        var result = response.Result as NoContentResult;

        Assert.NotNull(result);
        Assert.That(result!.StatusCode, Is.EqualTo(204));
    }

    [Test]
    public void GetAllTenants_ShouldThrowException_WhenExceptionThrownIsService()
    {
        Service.GetAllTenantsAsync().Returns(Task.FromResult(new GetMultipleResponse<TenantToGetDto>
            { Collection = null, Count = 0, Error = new Exception(), TimeStamp = DateTime.Now }));

        Assert.ThrowsAsync<Exception>(async () => await Controller.GetAllTenants());
    }
}