using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.BLL.Services.Contracts;

public interface IRoomService
{
    Task<Response<IEnumerable<RoomToGetDto>>> GetAllRoomsAsync();

    Task<Response<IEnumerable<RoomToGetDto>>> GetPartialRoomsAsync(GetPartialRequest request);

    Task<Response<IEnumerable<RoomTypeToGetDto>>> GetAllRoomTypesAsync();

    Task<Response<RoomToGetDto>> GetRoomByRoomIdAsync(Guid roomId);

    Task<Response<RoomToGetDto>> GetRoomByNumberAsync(int roomNumber);

    Task<Response<IEnumerable<AccommodationRoomToGetDto>>> GetAccommodationRoomsByRoomIdAsync(Guid roomId);

    Task<Response<AccommodationRoomToGetDto>> GetAccommodationRoomByIdAsync(Guid accommodationRoomId);

    Task<Response<Guid>> CreateRoomAsync(RoomToCreateDto room);

    Task<Response<int>> CreateRoomTypeAsync(RoomTypeToCreateDto roomType);

    Task<Response<Guid>> CreateAccommodationRoomAsync(AccommodationRoomToCreateDto accommodationRoom);

    Task<Response<int>> CreateAccommodationAsync(AccommodationToCreateDto accommodation);

    Task<Response<EntityEntry<Room>>> DeleteRoomAsync(Guid roomId);

    Task<Response<EntityEntry<AccommodationRoom>>> DeleteAccommodationRoomAsync(Guid accommodationRoomId);

    Task<Response<IEnumerable<AccommodationToGetDto>>> GetAllAccommodationsAsync();
    
    Task<Response<EntityEntry<AccommodationRoom>>> UpdateAccommodationRoom(AccommodationRoomToGetDto accommodationRoom);
}