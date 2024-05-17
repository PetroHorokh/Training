using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RepositoryBase;
using Rent.Response.Library;

namespace Rent.DAL.Repositories.Contracts;

public interface IAccommodationRoomRepository : IRepositoryBase<AccommodationRoom>
{
    Task<Response<Guid>> CreateWithProcedure(AccommodationRoomToCreateDto accommodationRoom);
}