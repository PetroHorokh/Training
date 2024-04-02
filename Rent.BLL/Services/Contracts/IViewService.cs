using Microsoft.EntityFrameworkCore;
using Rent.DAL.DTO;
using temp;

namespace Rent.BLL.Services.Contracts;

public interface IViewService
{
    Task<IEnumerable<VwCertificateForTenantToGetDto>> GetCertificateForTenantAsync(Guid tenantId);

    Task<IEnumerable<VwRoomsWithTenantToGetDto>> GetRoomsWithTenantsAsync(DateTime dateTime);

    Task<IEnumerable<VwTenantAssetPaymentToGetDto>> GetTenantAssetPaymentAsync(Guid tenantId);
}