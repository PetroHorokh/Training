using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RepositoryBase;

namespace Rent.DAL.Repositories.Contracts;

public interface IImpostRepository : IRepositoryBase<Impost>
{
    Task CreateWithProcedure(ImpostToCreateDto impost);
}