using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Responses;

namespace Rent.BLL.Services.Contracts;

public interface IRoomService
{
    Task<IEnumerable<RoomToGetDto>> GetAllRoomsAsync();

    Task<IEnumerable<RoomTypeToGetDto>> GetAllRoomTypesAsync();

    Task<RoomToGetDto?> GetRoomByRoomIdAsync(Guid roomId);

    Task<RoomToGetDto?> GetRoomByNumberAsync(int roomNumber);

    Task<IEnumerable<AccommodationRoomToGetDto>> GetAccommodationsOfRoomAsync(Guid roomId);

    Task<AccommodationRoomToGetDto?> GetAccommodationRoomByIdAsync(Guid accommodationRoomId);

    Task<CreationResponse> CreateRoomAsync(RoomToCreateDto room);

    Task<CreationDictionaryResponse> CreateRoomTypeAsync(RoomTypeToCreateDto roomType);

    Task<CreationResponse> CreateAccommodationRoomAsync(AccommodationRoomToCreateDto accommodationRoom);

    Task<CreationDictionaryResponse> CreateAccommodationAsync(AccommodationToCreateDto accommodation);

    Task<UpdatingResponse> DeleteRoomAsync(Guid roomId);

    Task<UpdatingResponse> DeleteAccommodationRoomAsync(Guid accommodationRoomId);

    Task<IEnumerable<AccommodationToGetDto>> GetAllAccommodationsAsync();
    
    Task<UpdatingResponse> UpdateAccommodationRoom(AccommodationRoomToUpdateDto accommodationRoom);
}