using Rent.DAL.RepositoryBase;
using Rent.DTOs.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories.Contracts;

public interface IRentRepository : IRepositoryBase<Model.Library.Rent>
{
    Task<Response<Guid>> CreateWithProcedure(RentToCreateDto rent);
}