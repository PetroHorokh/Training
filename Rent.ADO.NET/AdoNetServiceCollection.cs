using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rent.ADO.NET.Services;
using Rent.ADO.NET.Services.Contracts;

namespace Rent.ADO.NET;

public static class AdoNetServiceCollection
{
    public static IServiceCollection AdoNetServiceInject(this IServiceCollection services)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        services.AddSingleton<IConfiguration>(provider => config);

        services.AddSingleton<IDisconnectedArchitecture, DisconnectedArchitecture>();
        services.AddSingleton<IConnectedArchitecture, ConnectedArchitecture>();

        return services;
    }
}