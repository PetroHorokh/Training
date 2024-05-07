using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rent.Auth.DAL.Context;
using Serilog;

namespace Rent.Auth.DAL;

/// <summary>
/// Class for injection Auth.Dal dependencies into service collection
/// </summary>
public static class AuthDalServiceCollection
{
    /// <summary>
    /// Auth.Dal dependencies inject into service collection
    /// </summary>
    /// <param name="services">Service collection for injection additional dependencies</param>
    /// <returns>Enriched with Auth.Dal dependencies service collection</returns>
    public static IServiceCollection AuthDalServiceInject(this IServiceCollection services)
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

        services.AddDbContext<AuthRentContext>(option
            => option.UseSqlServer(config["ConnectionStrings:RentDatabase"]));

        return services;
    }
}