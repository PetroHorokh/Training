using Rent.DAL.RepositoryBase;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories.Contracts;

public interface ITenantRepository : IRepositoryBase<Tenant>
{
    Task<Response<Guid>> CreateWithProcedure(TenantToCreateDto tenant);
}