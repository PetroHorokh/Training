using Rent.DAL.RepositoryBase;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories.Contracts;

public interface IRoomTypeRepository : IRepositoryBase<RoomType>
{
    Task<Response<int>> CreateWithProcedure(RoomTypeToCreateDto roomType);
}