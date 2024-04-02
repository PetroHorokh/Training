using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rent.BLL.Services.Contracts;
using Rent.BLL.Services;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.Repositories;
using Rent.DAL.UnitOfWork;
using Rent.DAL;
using Serilog;
namespace Rent.BLL;

public static class BllServiceProvider
{
    public static ServiceProvider ServiceConfiguration()
    {
        var serviceProvider = new ServiceCollection();

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        serviceProvider.AddSingleton<IConfiguration>(provider => config);
        serviceProvider.AddLogging(builder => builder.AddSerilog(dispose: true));

        serviceProvider.AddDbContext<RentContext>(option
            => option.UseSqlServer(config["ConnectionStrings:RentDatabase"]));

        serviceProvider.AddSingleton<IAccommodationRepository, AccommodationRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IAccommodationRepository>(
                () => provider.GetService<IAccommodationRepository>()!,
                LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IAccommodationRoomRepository, AccommodationRoomRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IAccommodationRoomRepository>(
            () => provider.GetService<IAccommodationRoomRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IAddressRepository, AddressRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IAddressRepository>(
            () => provider.GetService<IAddressRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IAssetRepository, AssetRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IAssetRepository>(
            () => provider.GetService<IAssetRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IBillRepository, BillRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IBillRepository>(
            () => provider.GetService<IBillRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IImpostRepository, ImpostRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IImpostRepository>(
            () => provider.GetService<IImpostRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IOwnerRepository, OwnerRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IOwnerRepository>(
            () => provider.GetService<IOwnerRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IPaymentRepository, PaymentRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IPaymentRepository>(
            () => provider.GetService<IPaymentRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IPriceRepository, PriceRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IPriceRepository>(
            () => provider.GetService<IPriceRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IRentRepository, RentRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IRentRepository>(
            () => provider.GetService<IRentRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IRoomRepository, RoomRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IRoomRepository>(
            () => provider.GetService<IRoomRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IRoomTypeRepository, RoomTypeRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IRoomTypeRepository>(
            () => provider.GetService<IRoomTypeRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<ITenantRepository, TenantRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<ITenantRepository>(
            () => provider.GetService<ITenantRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IViewRepository, ViewRepository>();
        serviceProvider.AddSingleton(provider => new Lazy<IViewRepository>(
            () => provider.GetService<IViewRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        serviceProvider.AddSingleton<IUnitOfWork, UnitOfWork>();

        serviceProvider.AddSingleton<ITenantService, TenantService>();
        serviceProvider.AddSingleton<IRoomService, RoomService>();
        serviceProvider.AddSingleton<IOwnerService, OwnerService>();
        serviceProvider.AddSingleton<IViewService, ViewService>();

        serviceProvider.AddAutoMapper(typeof(Rent.BLL.Profiles.MappingProfile));

        return serviceProvider.BuildServiceProvider();
    }
}