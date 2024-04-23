using Rent.DAL.DTO;
using Rent.DAL.RepositoryBase;
using Rent.DAL.RequestsAndResponses;

namespace Rent.DAL.Repositories.Contracts;

public interface IRentRepository : IRepositoryBase<Models.Rent>
{
    Task<CreationResponse> CreateWithProcedure(RentToCreateDto rent);
}