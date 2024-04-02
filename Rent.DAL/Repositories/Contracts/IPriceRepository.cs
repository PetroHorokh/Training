using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RepositoryBase;

namespace Rent.DAL.Repositories.Contracts;

public interface IPriceRepository : IRepositoryBase<Price>
{
    Task CreateWithProcedure(PriceToCreateDto price);
}