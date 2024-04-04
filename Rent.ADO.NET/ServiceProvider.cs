using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rent.ADO.NET.Services;
using Rent.ADO.NET.Services.Contracts;

namespace Rent.ADO.NET;

public static class AdoNetServiceProvider
{
    public static ServiceProvider ServiceConfiguration()
    {
        var serviceProvider = new ServiceCollection();

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        serviceProvider.AddSingleton<IConfiguration>(provider => config);

        serviceProvider.AddSingleton<IDisconnectedArchitecture, DisconnectedArchitecture>();
        serviceProvider.AddSingleton<IConnectedArchitecture, ConnectedArchitecture>();

        return serviceProvider.BuildServiceProvider();
    }
}