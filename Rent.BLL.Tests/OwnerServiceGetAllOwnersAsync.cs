using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RequestsAndResponses;
using Rent.WebAPI.CustomExceptions;

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
        Assert.Null(result.Error);
        Assert.That(result.Count == 2);
        Assert.That(result.Count, Is.EqualTo(result.Collection!.Count()));
    }

    [Test]
    public async Task
        GetAllOwnersAsync_ShouldReturnIEnumerableOfOwnerToGedDto_WhenIEnumerableOfOwnerToGedDtoNotPresentInMemoryCache()
    {
        UnitOfWork.Owners.GetAllAsync().Returns(Task.FromResult(new GetMultipleResponse<Owner>
        {
            Collection = new List<Owner> { new(), new() },
            Count = 2,
            Error = null,
            TimeStamp = DateTime.Now
        }));

        var result = await OwnerService.GetAllOwnersAsync();

        Assert.NotNull(result);
        Assert.Null(result.Error);
        Assert.That(result.Count == 2);
        Assert.That(result.Count, Is.EqualTo(result.Collection!.Count()));
    }

    [Test]
    public async Task GetAllOwnersAsync_ShouldReturnProcessException_WhenErrorOccuredRetrievingOwnerFromDatabase()
    {
        UnitOfWork.Owners.GetAllAsync().Returns(Task.FromResult(new GetMultipleResponse<Owner>
        {
            Collection = null,
            Count = null,
            Error = new Exception(),
            TimeStamp = DateTime.Now
        }));

        var result = await OwnerService.GetAllOwnersAsync();

        Assert.NotNull(result);
        Assert.That(result.Error, Is.TypeOf<ProcessException>());
        Assert.NotNull(result.Error);
        Assert.Null(result.Collection);
    }

    [Test]
    public async Task GetAllOwnersAsync_ShouldReturnAutoMapperMappingException_WhenErrorOccuredWhileMappingEntities()
    {
        UnitOfWork.Owners.GetAllAsync().Returns(Task.FromResult(new GetMultipleResponse<Owner>
        {
            Collection = new List<Owner> { new(), new() },
            Count = 2,
            Error = null,
            TimeStamp = DateTime.Now
        }));
        Mapper.When(x => x.Map<OwnerToGetDto>(Arg.Any<Owner>())).Do(x => throw new AutoMapperMappingException());

        MemoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<OwnerToGetDto>>()!).Returns(x => false);

        var result = await OwnerService.GetAllOwnersAsync();

        Assert.NotNull(result);
        Assert.That(result.Error, Is.TypeOf<AutoMapperMappingException>());
        Assert.NotNull(result.Error);
        Assert.Null(result.Collection);
    }
}