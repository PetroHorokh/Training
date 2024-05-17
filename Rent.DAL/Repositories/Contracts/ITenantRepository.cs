using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RepositoryBase;
using Rent.DAL.RequestsAndResponses;
using Rent.Response.Library;

namespace Rent.DAL.Repositories.Contracts;

public interface ITenantRepository : IRepositoryBase<Tenant>
{
    Task<Response<Guid>> CreateWithProcedure(TenantToCreateDto tenant);
}