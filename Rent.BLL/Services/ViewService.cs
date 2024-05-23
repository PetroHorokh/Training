using AutoMapper;
using Microsoft.Extensions.Logging;
using Rent.BLL.Services.Contracts;
using Rent.DAL.UnitOfWork;
using Rent.DTOs.Library;

namespace Rent.BLL.Services;

public class ViewService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ViewService> logger) : IViewService
{
    public async Task<IEnumerable<VwCertificateForTenantToGetDto>> GetCertificateForTenantAsync(Guid tenantId)
    {
        logger.LogInformation("Entering ViewService, GetCertificateForTenantAsync");

        logger.LogInformation("Calling ViewRepository, method GetCertificateForTenantAsync");
        var result = (await unitOfWork
            .Views
            .GetCertificateForTenant(tenantId))
            .GroupBy(entity => entity.RentId)
            .Select(group => new VwCertificateForTenantToGetDto
            {
                RentId = group.Key,
                RentStartDate = group.Min(entity => entity.RentStartDate),
                RentEndDate = group.Max(entity => entity.RentStartDate),
                BillIds = string.Join(",\n", group.Select(obj => obj.BillId)),
                PaymentIds = string.Join(",\n", group.Where(obj => obj.PaymentId != null).Select(obj => obj.PaymentId)),
            }); ;
        logger.LogInformation("Finished calling ViewRepository, method GetCertificateForTenantAsync");

        logger.LogInformation("Exiting ViewService, GetCertificateForTenantAsync");
        return result.ToList();
    }

    public async Task<IEnumerable<VwRoomsWithTenantToGetDto>> GetRoomsWithTenantsAsync(DateTime dateTime)
    {
        logger.LogInformation("Entering ViewService, GetRoomsWithTenantsAsync");

        logger.LogInformation("Calling ViewRepository, method GetRoomsWithTenantsAsync");
        var entities = (await unitOfWork.Views.GetRoomsWithTenants(dateTime)).ToList();
        logger.LogInformation("Finished calling ViewRepository, method GetRoomsWithTenantsAsync");

        logger.LogInformation($"Mapping tenants to VwRoomsWithTenantToGetDto");
        var result = entities.Select(mapper.Map<VwRoomsWithTenantToGetDto>);

        logger.LogInformation("Exiting ViewService, GetRoomsWithTenantsAsync");
        return result;
    }

    public async Task<IEnumerable<VwTenantAssetPaymentToGetDto>> GetTenantAssetPaymentAsync(Guid tenantId)
    {
        logger.LogInformation("Entering ViewService, GetTenantAssetPaymentAsync");

        logger.LogInformation("Calling ViewRepository, method GetTenantAssetPaymentAsync");
        var entities = (await unitOfWork.Views.GetTenantAssetPayment(tenantId)).ToList();
        logger.LogInformation("Finished calling ViewRepository, method GetTenantAssetPaymentAsync");

        logger.LogInformation($"Mapping tenants to VwRoomsWithTenantToGetDto");
        var result = entities.Select(mapper.Map<VwTenantAssetPaymentToGetDto>);

        logger.LogInformation("Exiting ViewService, GetTenantAssetPaymentAsync");
        return result;
    }
}