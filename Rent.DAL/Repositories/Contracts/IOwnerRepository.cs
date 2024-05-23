using Rent.DAL.RepositoryBase;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories.Contracts;

public interface IOwnerRepository : IRepositoryBase<Owner>
{
    Task<Response<Guid>> CreateWithProcedure(OwnerToCreateDto owner);
}