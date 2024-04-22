using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rent.DAL.Context;
using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.Repositories;
using Rent.DAL.RepositoryBase;
using Rent.DAL.UnitOfWork;
using Serilog;

namespace Rent.DAL;

public static class DalServiceCollection
{
    public static IServiceCollection DalServiceInject(this IServiceCollection services)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("./appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        services.AddSingleton<IConfiguration>(provider => config);
        services.AddLogging(builder => builder.AddSerilog(dispose: true));

        services.AddDbContext<RentContext>(option
            => option.UseSqlServer(config["ConnectionStrings:RentDatabase"]));

        services.AddScoped<IAccommodationRepository, AccommodationRepository>();
        services.AddScoped(provider => new Lazy<IAccommodationRepository>(
                () => provider.GetService<IAccommodationRepository>()!,
                LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IAccommodationRoomRepository, AccommodationRoomRepository>();
        services.AddScoped(provider => new Lazy<IAccommodationRoomRepository>(
            () => provider.GetService<IAccommodationRoomRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped(provider => new Lazy<IAddressRepository>(
            () => provider.GetService<IAddressRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped(provider => new Lazy<IAssetRepository>(
            () => provider.GetService<IAssetRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped(provider => new Lazy<IBillRepository>(
            () => provider.GetService<IBillRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IImpostRepository, ImpostRepository>();
        services.AddScoped(provider => new Lazy<IImpostRepository>(
            () => provider.GetService<IImpostRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IOwnerRepository, OwnerRepository>();
        services.AddScoped(provider => new Lazy<IOwnerRepository>(
            () => provider.GetService<IOwnerRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped(provider => new Lazy<IPaymentRepository>(
            () => provider.GetService<IPaymentRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IPriceRepository, PriceRepository>();
        services.AddScoped(provider => new Lazy<IPriceRepository>(
            () => provider.GetService<IPriceRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IRentRepository, RentRepository>();
        services.AddScoped(provider => new Lazy<IRentRepository>(
            () => provider.GetService<IRentRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped(provider => new Lazy<IRoomRepository>(
            () => provider.GetService<IRoomRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
        services.AddScoped(provider => new Lazy<IRoomTypeRepository>(
            () => provider.GetService<IRoomTypeRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped(provider => new Lazy<ITenantRepository>(
            () => provider.GetService<ITenantRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(provider => new Lazy<IUserRepository>(
            () => provider.GetService<IUserRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IRepositoryBase<Role>, RepositoryBase<Role>>();
        services.AddScoped(provider => new Lazy<IRepositoryBase<Role>>(
            () => provider.GetService<IRepositoryBase<Role>>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IRepositoryBase<UserRole>, RepositoryBase<UserRole>>();
        services.AddScoped(provider => new Lazy<IRepositoryBase<UserRole>>(
            () => provider.GetService<IRepositoryBase<UserRole>>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IViewRepository, ViewRepository>();
        services.AddScoped(provider => new Lazy<IViewRepository>(
            () => provider.GetService<IViewRepository>()!,
            LazyThreadSafetyMode.ExecutionAndPublication));

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}