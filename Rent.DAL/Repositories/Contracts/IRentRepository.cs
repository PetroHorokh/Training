using Rent.DAL.DTO;
using Rent.DAL.RepositoryBase;
using Rent.DAL.RequestsAndResponses;
using Rent.Response.Library;

namespace Rent.DAL.Repositories.Contracts;

public interface IRentRepository : IRepositoryBase<Models.Rent>
{
    Task<Response<Guid>> CreateWithProcedure(RentToCreateDto rent);
}