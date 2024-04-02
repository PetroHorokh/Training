using Rent.DAL.DTO;
using temp;

namespace Rent.DAL.Repositories.Contracts;

public interface IViewRepository
{
    Task<IEnumerable<VwCertificateForTenant>> GetCertificateForTenant(Guid tenantId);

    Task<IEnumerable<VwRoomsWithTenant>> GetRoomsWithTenants(DateTime dateTime);

    Task<IEnumerable<VwTenantAssetPayment>> GetTenantAssetPayment(Guid tenantId);
}