using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Rent.BLL.Services;
using Rent.BLL.Services.Contracts;
using Rent.DAL.UnitOfWork;

namespace Rent.BLL.Tests;

public class SetUp
{
    public static readonly IUnitOfWork UnitOfWork = Substitute.For<IUnitOfWork>();
    public static readonly IMapper Mapper = Substitute.For<IMapper>();
    public static readonly ILogger<TenantService> TenantLogger = Substitute.For<ILogger<TenantService>>();
    public static readonly ILogger<OwnerService> OwnerLogger = Substitute.For<ILogger<OwnerService>>();
    public static readonly IMemoryCache MemoryCache = Substitute.For<IMemoryCache>();
    public static readonly ITenantService TenantService = new TenantService(UnitOfWork, Mapper, TenantLogger, MemoryCache);
    public static readonly IOwnerService OwnerService = new OwnerService(UnitOfWork, Mapper, OwnerLogger, MemoryCache);

    [OneTimeSetUp]
    public void Setup()
    {
    }

    [TearDown]
    public void Teardown()
    {
        UnitOfWork.ClearSubstitute();
        Mapper.ClearSubstitute();
        MemoryCache.ClearSubstitute();
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        UnitOfWork.Dispose();
        MemoryCache.Dispose();
    }
}