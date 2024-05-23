﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rent.BLL.Profiles;
using Rent.BLL.Services.Contracts;
using Rent.BLL.Services;
using Rent.DAL;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.Repositories;
using Rent.DAL.UnitOfWork;
using Serilog;
using Rent.DAL.Context;
namespace Rent.BLL;

public static class BllServiceCollection
{
    public static IServiceCollection BllServiceInject(this IServiceCollection services)
    {
        services.AddLogging(builder => builder.AddSerilog(dispose: true));

        services.AddMemoryCache();

        services.DalServiceInject();

        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IOwnerService, OwnerService>();
        services.AddScoped<IViewService, ViewService>();

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}