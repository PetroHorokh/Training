using Rent.DAL.RepositoryBase;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories.Contracts;

public interface IAccommodationRoomRepository : IRepositoryBase<AccommodationRoom>
{
    Task<Response<Guid>> CreateWithProcedure(AccommodationRoomToCreateDto accommodationRoom);
}