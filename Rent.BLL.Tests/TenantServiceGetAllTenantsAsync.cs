using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

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
        Assert.IsEmpty(result.Exceptions);
        Assert.That(result.Body!.Count() == 2);
    }

    [Test]
    public async Task
        GetAllTenantsAsync_ShouldReturnIEnumerableOfTenantToGedDto_WhenIEnumerableOfTenantToGedDtoNotPresentInMemoryCache()
    {
        UnitOfWork.Tenants.GetAllAsync().Returns(Task.FromResult(new Response<IEnumerable<Tenant>>()
        {
            Body = new List<Tenant> { new(), new() },
        }));

        var result = await TenantService.GetAllTenantsAsync();

        Assert.NotNull(result);
        Assert.IsEmpty(result.Exceptions);
        Assert.That(result.Body!.Count() == 2);
    }

    [Test]
    public async Task GetAllTenantsAsync_ShouldReturnProcessException_WhenErrorOccuredRetrievingTenantFromDatabase()
    {
        UnitOfWork.Tenants.GetAllAsync().Returns(Task.FromResult(new Response<IEnumerable<Tenant>>
        {
            Exceptions = [new()],
        }));

        var result = await TenantService.GetAllTenantsAsync();

        Assert.NotNull(result);
        Assert.That(result.Exceptions.ElementAt(0), Is.TypeOf<Exception>());
        Assert.IsNotEmpty(result.Exceptions);
        Assert.IsEmpty(result.Body!);
    }

    [Test]
    public async Task GetAllTenantsAsync_ShouldReturnAutoMapperMappingException_WhenErrorOccuredWhileMappingEntities()
    {
        UnitOfWork.Tenants.GetAllAsync().Returns(Task.FromResult(new Response<IEnumerable<Tenant>>()
        {
            Body = new List<Tenant> { new(), new() },
        }));
        Mapper.When(x => x.Map<TenantToGetDto>(Arg.Any<Tenant>())).Do(x => throw new AutoMapperMappingException());

        MemoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<TenantToGetDto>>()!).Returns(x => false);

        var result = await TenantService.GetAllTenantsAsync();

        Assert.NotNull(result);
        Assert.That(result.Exceptions.ElementAt(0), Is.TypeOf<AutoMapperMappingException>());
        Assert.IsNotEmpty(result.Exceptions);
        Assert.Null(result.Body);
    }
}