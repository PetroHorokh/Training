using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RepositoryBase;
using Rent.DAL.RequestsAndResponses;

namespace Rent.DAL.Repositories.Contracts;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<CreationResponse> CreateWithProcedure(UserToCreateDto accommodation);
}