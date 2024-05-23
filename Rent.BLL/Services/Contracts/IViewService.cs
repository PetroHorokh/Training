using Rent.DTOs.Library;

namespace Rent.BLL.Services.Contracts;

public interface IViewService
{
    Task<IEnumerable<VwCertificateForTenantToGetDto>> GetCertificateForTenantAsync(Guid tenantId);

    Task<IEnumerable<VwRoomsWithTenantToGetDto>> GetRoomsWithTenantsAsync(DateTime dateTime);

    Task<IEnumerable<VwTenantAssetPaymentToGetDto>> GetTenantAssetPaymentAsync(Guid tenantId);
}