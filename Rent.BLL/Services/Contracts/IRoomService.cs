using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RequestsAndResponses;

namespace Rent.BLL.Services.Contracts;

public interface IRoomService
{
    Task<GetMultipleResponse<RoomToGetDto>> GetAllRoomsAsync();

    Task<GetMultipleResponse<RoomToGetDto>> GetPartialRoomsAsync(GetPartialRequest request);

    Task<GetMultipleResponse<RoomTypeToGetDto>> GetAllRoomTypesAsync();

    Task<GetSingleResponse<RoomToGetDto>> GetRoomByRoomIdAsync(Guid roomId);

    Task<GetSingleResponse<RoomToGetDto>> GetRoomByNumberAsync(int roomNumber);

    Task<GetMultipleResponse<AccommodationRoomToGetDto>> GetAccommodationRoomsByRoomIdAsync(Guid roomId);

    Task<GetSingleResponse<AccommodationRoomToGetDto>> GetAccommodationRoomByIdAsync(Guid accommodationRoomId);

    Task<CreationResponse> CreateRoomAsync(RoomToCreateDto room);

    Task<CreationDictionaryResponse> CreateRoomTypeAsync(RoomTypeToCreateDto roomType);

    Task<CreationResponse> CreateAccommodationRoomAsync(AccommodationRoomToCreateDto accommodationRoom);

    Task<CreationDictionaryResponse> CreateAccommodationAsync(AccommodationToCreateDto accommodation);

    Task<ModifyResponse<Room>> DeleteRoomAsync(Guid roomId);

    Task<ModifyResponse<AccommodationRoom>> DeleteAccommodationRoomAsync(Guid accommodationRoomId);

    Task<GetMultipleResponse<AccommodationToGetDto>> GetAllAccommodationsAsync();
    
    Task<ModifyResponse<AccommodationRoom>> UpdateAccommodationRoom(AccommodationRoomToGetDto accommodationRoom);
}