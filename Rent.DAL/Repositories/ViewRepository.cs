using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rent.DAL.Context;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using System;
using temp;

namespace Rent.DAL.Repositories;

public class ViewRepository(RentContext context, ILogger<ViewRepository> logger) : IViewRepository
{
    private readonly RentContext _context = context;
    private readonly ILogger<ViewRepository> _logger = logger;

    public async Task<IEnumerable<VwCertificateForTenant>> GetCertificateForTenant(Guid tenantId)
    {
        _logger.LogInformation("Entering ViewRepository, GetCertificateForTenant");

        _logger.LogInformation("Querying calling 'vw_Certificate_For_Tenants' view");
        _logger.LogInformation($"Parameter: tenantId = {tenantId}");
        var result = await _context.VwCertificateForTenants.Where(tenant => tenant.TenantId == tenantId).ToListAsync();
        _logger.LogInformation("Queried view successfully");

        _logger.LogInformation("Exiting ViewRepository, GetCertificateForTenant");
        return result;
    }
    public async Task<IEnumerable<VwRoomsWithTenant>> GetRoomsWithTenants(DateTime dateTime)
    {
        _logger.LogInformation("Entering ViewRepository, GetRoomsWithTenants");

        _logger.LogInformation("Querying calling 'vw_Rooms_With_Tenants' view");
        _logger.LogInformation($"Parameter: dateTime = {dateTime}");
        var result = await _context
            .VwRoomsWithTenants.Where(rent => dateTime >= rent.StartDate && dateTime <= rent.EndDate).ToListAsync();
        _logger.LogInformation("Queried view successfully");

        _logger.LogInformation("Exiting ViewRepository, GetRoomsWithTenants");
        return result;
    }
    public async Task<IEnumerable<VwTenantAssetPayment>> GetTenantAssetPayment(Guid tenantId)
    {
        _logger.LogInformation("Entering ViewRepository, GetTenantAssetPayment");

        _logger.LogInformation("Querying calling 'vw_Tenant_Asset_Payments' view");
        _logger.LogInformation($"Parameter: tenantId = {tenantId}");
        var result = await _context.VwTenantAssetPayments.Where(tenant => tenant.TenantId == tenantId).ToListAsync();
        _logger.LogInformation("Queried view successfully");

        _logger.LogInformation("Exiting ViewRepository, GetTenantAssetPayment");
        return result;
    }
}