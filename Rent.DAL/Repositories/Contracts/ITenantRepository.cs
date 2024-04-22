using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RepositoryBase;
using Rent.DAL.RequestsAndResponses;
using Rent.DAL.Responses;

namespace Rent.DAL.Repositories.Contracts;

public interface ITenantRepository : IRepositoryBase<Tenant>
{
    Task<CreationResponse> CreateWithProcedure(TenantToCreateDto tenant);

    Task<IEnumerable<Tenant>> GetTenantsPartialAsync(GetRequest request);
}