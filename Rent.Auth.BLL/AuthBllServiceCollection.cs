using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rent.Auth.BLL.Services;
using Rent.Auth.BLL.Services.Contracts;
using Rent.Auth.DAL;
using Serilog;

namespace Rent.Auth.BLL;

public static class AuthBllServiceCollection
{
    public static IServiceCollection AuthBllServiceInject(this IServiceCollection services)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        services.AddScoped<IConfiguration>(provider => config);
        services.AddLogging(builder => builder.AddSerilog(dispose: true));

        services.AuthDalServiceInject();

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}