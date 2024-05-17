using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RequestsAndResponses;
using Rent.ExceptionLibrary;
using Rent.Response.Library;

namespace Rent.BLL.Tests;

public class OwnerServiceGetAllOwnersAsync : SetUp
{
    [Test]
    public async Task
        GetAllOwnersAsync_ShouldReturnIEnumerableOfOwnerToGedDto_WhenIEnumerableOfOwnerToGedDtoPresentInMemoryCache()
    {
        MemoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<OwnerToGetDto>>()!).Returns(x =>
        {
            x[1] = new List<OwnerToGetDto> { new(), new() };
            return true;
        });

        var result = await OwnerService.GetAllOwnersAsync();

        Assert.NotNull(result);
        Assert.IsEmpty(result.Exceptions);
        Assert.That(result.Body!.Count() == 2);
    }

    [Test]
    public async Task
        GetAllOwnersAsync_ShouldReturnIEnumerableOfOwnerToGedDto_WhenIEnumerableOfOwnerToGedDtoNotPresentInMemoryCache()
    {
        UnitOfWork.Owners.GetAllAsync().Returns(Task.FromResult(new Response<IEnumerable<Owner>>()
        {
            Body = new List<Owner> { new(), new() },
        }));

        var result = await OwnerService.GetAllOwnersAsync();

        Assert.NotNull(result);
        Assert.IsEmpty(result.Exceptions);
        Assert.That(result.Body!.Count() == 2);
    }

    [Test]
    public async Task GetAllOwnersAsync_ShouldReturnProcessException_WhenErrorOccuredRetrievingOwnerFromDatabase()
    {
        UnitOfWork.Owners.GetAllAsync().Returns(Task.FromResult(new Response<IEnumerable<Owner>>()
        {
            Exceptions = [new Exception()],
        }));

        var result = await OwnerService.GetAllOwnersAsync();

        Assert.NotNull(result);
        Assert.That(result.Exceptions.ElementAt(0), Is.TypeOf<Exception>());
        Assert.IsNotEmpty(result.Exceptions);
        Assert.IsEmpty(result.Body!);
    }

    [Test]
    public async Task GetAllOwnersAsync_ShouldReturnAutoMapperMappingException_WhenErrorOccuredWhileMappingEntities()
    {
        UnitOfWork.Owners.GetAllAsync().Returns(Task.FromResult(new Response<IEnumerable<Owner>>()
        {
            Body = new List<Owner> { new(), new() },
        }));
        Mapper.When(x => x.Map<OwnerToGetDto>(Arg.Any<Owner>())).Do(x => throw new AutoMapperMappingException());

        MemoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<OwnerToGetDto>>()!).Returns(x => false);

        var result = await OwnerService.GetAllOwnersAsync();

        Assert.NotNull(result);
        Assert.That(result.Exceptions.ElementAt(0), Is.TypeOf<AutoMapperMappingException>());
        Assert.IsNotEmpty(result.Exceptions);
        Assert.Null(result.Body);
    }
}