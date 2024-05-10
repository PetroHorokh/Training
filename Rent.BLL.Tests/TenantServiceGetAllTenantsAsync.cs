using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RequestsAndResponses;
using Rent.WebAPI.CustomExceptions;

namespace Rent.BLL.Tests;

public class TenantServiceGetAllTenantsAsync : SetUp
{
    [Test]
    public async Task
        GetAllTenantsAsync_ShouldReturnIEnumerableOfTenantToGedDto_WhenIEnumerableOfTenantToGedDtoPresentInMemoryCache()
    {
        MemoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<TenantToGetDto>>()!).Returns(x =>
        {
            x[1] = new List<TenantToGetDto> { new(), new() };
            return true;
        });

        var result = await TenantService.GetAllTenantsAsync();

        Assert.NotNull(result);
        Assert.Null(result.Error);
        Assert.That(result.Count == 2);
        Assert.That(result.Count, Is.EqualTo(result.Collection!.Count()));
    }

    [Test]
    public async Task
        GetAllTenantsAsync_ShouldReturnIEnumerableOfTenantToGedDto_WhenIEnumerableOfTenantToGedDtoNotPresentInMemoryCache()
    {
        UnitOfWork.Tenants.GetAllAsync().Returns(Task.FromResult(new GetMultipleResponse<Tenant>
        {
            Collection = new List<Tenant> { new(), new() },
            Count = 2,
            Error = null,
            TimeStamp = DateTime.Now
        }));

        var result = await TenantService.GetAllTenantsAsync();

        Assert.NotNull(result);
        Assert.Null(result.Error);
        Assert.That(result.Count == 2);
        Assert.That(result.Count, Is.EqualTo(result.Collection!.Count()));
    }

    [Test]
    public async Task GetAllTenantsAsync_ShouldReturnProcessException_WhenErrorOccuredRetrievingTenantFromDatabase()
    {
        UnitOfWork.Tenants.GetAllAsync().Returns(Task.FromResult(new GetMultipleResponse<Tenant>
        {
            Collection = null,
            Count = null,
            Error = new Exception(),
            TimeStamp = DateTime.Now
        }));

        var result = await TenantService.GetAllTenantsAsync();

        Assert.NotNull(result);
        Assert.That(result.Error, Is.TypeOf<ProcessException>());
        Assert.NotNull(result.Error);
        Assert.Null(result.Collection);
    }

    [Test]
    public async Task GetAllTenantsAsync_ShouldReturnAutoMapperMappingException_WhenErrorOccuredWhileMappingEntities()
    {
        UnitOfWork.Tenants.GetAllAsync().Returns(Task.FromResult(new GetMultipleResponse<Tenant>
        {
            Collection = new List<Tenant> { new(), new() },
            Count = 2,
            Error = null,
            TimeStamp = DateTime.Now
        }));
        Mapper.When(x => x.Map<TenantToGetDto>(Arg.Any<Tenant>())).Do(x => throw new AutoMapperMappingException());

        MemoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<TenantToGetDto>>()!).Returns(x => false);

        var result = await TenantService.GetAllTenantsAsync();

        Assert.NotNull(result);
        Assert.That(result.Error, Is.TypeOf<AutoMapperMappingException>());
        Assert.NotNull(result.Error);
        Assert.Null(result.Collection);
    }
}