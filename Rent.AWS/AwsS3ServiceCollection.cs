using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rent.AWS.S3.Services;
using Rent.AWS.S3.Services.Contracts;
using Serilog;

namespace Rent.AWS.S3;

public static class AwsS3ServiceCollection
{
    public static IServiceCollection AwsS3ServiceInject(this IServiceCollection services)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        services.AddSingleton<IConfiguration>(provider => config);
        services.AddLogging(builder => builder.AddSerilog(dispose: true));

        services.AddScoped<IS3Service, S3Service>();

        return services;
    }
}