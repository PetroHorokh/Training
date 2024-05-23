using Rent.DAL.RepositoryBase;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories.Contracts;

public interface IRoomRepository : IRepositoryBase<Room>
{
    Task<Response<Guid>> CreateWithProcedure(RoomToCreateDto room);
}