using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.UnitOfWork;
using System.Data.SqlTypes;
using Rent.DAL.RequestsAndResponses;

namespace Rent.BLL.Services;

public class RoomService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OwnerService> logger) : IRoomService
{
    public async Task<GetMultipleResponse<RoomToGetDto>> GetAllRoomsAsync()
    {
        var result = new GetMultipleResponse<RoomToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetAllRoomsAsync");

            logger.LogInformation("Calling RoomRepository, method GetAllAsync");
            var response = await unitOfWork.Rooms.GetAllAsync();
            logger.LogInformation("Finished calling RoomRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping rooms to RoomToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<RoomToGetDto>);

            logger.LogInformation("Exiting RoomService, GetAllRoomsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<RoomToGetDto>> GetPartialRoomsAsync(GetPartialRequest request)
    {
        var result = new GetMultipleResponse<RoomToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetPartialRoomsAsync");

            logger.LogInformation("Calling RoomRepository, method GetAllAsync");
            var response = await unitOfWork.Rooms.GetPartialAsync(request.Skip, request.Take);
            logger.LogInformation("Finished calling RoomRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping rooms to RoomToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<RoomToGetDto>);

            logger.LogInformation("Exiting RoomService, GetPartialRoomsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<RoomTypeToGetDto>> GetAllRoomTypesAsync()
    {
        var result = new GetMultipleResponse<RoomTypeToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetAllRoomTypesAsync");

            logger.LogInformation("Calling RoomTypeRepository, method GetAllAsync");
            var response = await unitOfWork.RoomTypes.GetAllAsync();
            logger.LogInformation("Finished calling RoomTypeRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping room types to RoomToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<RoomTypeToGetDto>);

            logger.LogInformation("Exiting RoomService, GetAllRoomTypesAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<AccommodationToGetDto>> GetAllAccommodationsAsync()
    {
        var result = new GetMultipleResponse<AccommodationToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetAllAccommodationsAsync");

            logger.LogInformation("Calling AccommodationRepository, method GetAllAsync");
            var response = await unitOfWork.Accommodations.GetAllAsync();
            logger.LogInformation("Finished calling AccommodationRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping accommodations to AccommodationToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<AccommodationToGetDto>);

            logger.LogInformation("Exiting RoomService, GetAllAccommodationsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetSingleResponse<RoomToGetDto>> GetRoomByRoomIdAsync(Guid roomId)
    {
        var result = new GetSingleResponse<RoomToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetRoomByRoomIdAsync");

            logger.LogInformation("Calling RoomRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Rooms.GetSingleByConditionAsync(room => room.RoomId == roomId);
            logger.LogInformation("Finished calling RoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping room to RoomToGetDto");
            result.Entity = mapper.Map<RoomToGetDto>(response.Entity);

            logger.LogInformation("Exiting RoomService, GetRoomByRoomIdAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetSingleResponse<RoomToGetDto>> GetRoomByNumberAsync(int roomNumber)
    {
        var result = new GetSingleResponse<RoomToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetRoomByNumberAsync");

            logger.LogInformation("Calling RoomRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Rooms.GetSingleByConditionAsync(room => room.Number == roomNumber);
            logger.LogInformation("Finished calling RoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping room to RoomToGetDto");
            result.Entity = mapper.Map<RoomToGetDto>(response.Entity);

            logger.LogInformation("Exiting RoomService, GetRoomByNumberAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetSingleResponse<AccommodationRoomToGetDto>> GetAccommodationRoomByIdAsync(Guid accommodationRoomId)
    {
        var result = new GetSingleResponse<AccommodationRoomToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetAccommodationRoomByIdAsync");

            logger.LogInformation("Calling AccommodationRoomRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.AccommodationRooms.GetSingleByConditionAsync(accommodationRoom =>
                accommodationRoom.AccommodationRoomId == accommodationRoomId);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping room accommodation to AccommodationRoomToGetDto");
            result.Entity = mapper.Map<AccommodationRoomToGetDto>(response.Entity);

            logger.LogInformation("Exiting RoomService, GetAccommodationRoomByIdAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<AccommodationRoomToGetDto>> GetAccommodationRoomsByRoomIdAsync(Guid roomId)
    {
        var result = new GetMultipleResponse<AccommodationRoomToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetAccommodationRoomsByRoomIdAsync");

            logger.LogInformation("Calling AccommodationRoomRepository, method GetByConditionAsync");
            var response =
                await unitOfWork.AccommodationRooms.GetByConditionAsync(accommodationRoom =>
                    accommodationRoom.RoomId == roomId);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping room accommodations to AccommodationRoomToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<AccommodationRoomToGetDto>);

            logger.LogInformation("Exiting RoomService, GetAccommodationRoomsByRoomIdAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<CreationResponse> CreateRoomAsync(RoomToCreateDto room)
    {
        logger.LogInformation("Entering RoomService, CreateRoom");

        logger.LogInformation("Calling RoomRepository, method CreateWithProcedure");
        logger.LogInformation(
            $"Parameters: @Number = {room.Number}, @Area = {room.Area}, @RoomTypeId = {room.RoomTypeId}");
        var result = await unitOfWork.Rooms.CreateWithProcedure(room);
        logger.LogInformation("Finished calling RoomRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting RoomService, CreateRoom");
        return result;
    }

    public async Task<CreationDictionaryResponse> CreateRoomTypeAsync(RoomTypeToCreateDto roomType)
    {
        logger.LogInformation("Entering RoomService, CreateRoomTypeAsync");

        logger.LogInformation("Calling RoomTypeRepository, method CreateWithProcedure");
        logger.LogInformation(
            $"Parameters: @Name = {roomType.Name}");
        var result = await unitOfWork.RoomTypes.CreateWithProcedure(roomType);
        logger.LogInformation("Finished calling RoomTypeRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting RoomService, CreateRoomTypeAsync");
        return result;
    }

    public async Task<CreationResponse> CreateAccommodationRoomAsync(AccommodationRoomToCreateDto accommodationRoom)
    {
        logger.LogInformation("Entering RoomService, CreateAccommodationRoomAsync");

        logger.LogInformation("Calling AccommodationRoomRepository, method CreateWithProcedure");
        logger.LogInformation(
            $"Parameters: @AccommodationId = {accommodationRoom.AccommodationId}, @RoomId = {accommodationRoom.RoomId}, @Quantity = {accommodationRoom.Quantity}");
        var result = await unitOfWork.AccommodationRooms.CreateWithProcedure(accommodationRoom);
        logger.LogInformation("Finished calling AccommodationRoomRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting RoomService, CreateAccommodationRoomAsync");
        return result;
    }

    public async Task<CreationDictionaryResponse> CreateAccommodationAsync(AccommodationToCreateDto accommodation)
    {
        logger.LogInformation("Entering RoomService, CreateAccommodationAsync");

        logger.LogInformation("Calling AccommodationRepository, method CreateWithProcedure");
        logger.LogInformation($@"Parameters: @Name = {accommodation.Name}");
        var result = await unitOfWork.Accommodations.CreateWithProcedure(accommodation);
        logger.LogInformation("Finished calling AccommodationRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting RoomService, CreateAccommodationAsync");
        return result;
    }

    public async Task<ModifyResponse<Room>> DeleteRoomAsync(Guid roomId)
    {
        var result = new ModifyResponse<Room>();

        try
        {
            logger.LogInformation("Entering RoomService, DeleteRoomAsync");

            logger.LogInformation("Calling RoomRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.Rooms.GetSingleByConditionAsync(room => room.RoomId == roomId);
            logger.LogInformation("Finished calling RoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (response1.Error is not null)
            {
                throw response1.Error;
            }

            logger.LogInformation("Calling RoomRepository, method Delete");
            var response2 = unitOfWork.Rooms.Delete(response1.Entity!);
            logger.LogInformation("Finished calling RoomRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                throw response2.Error;
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting RoomService, DeleteRoomAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<ModifyResponse<AccommodationRoom>> DeleteAccommodationRoomAsync(Guid accommodationRoomId)
    {
        var result = new ModifyResponse<AccommodationRoom>();

        try
        {
            logger.LogInformation("Entering RoomService, DeleteAccommodationRoomAsync");

            logger.LogInformation("Calling AccommodationRoomRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.AccommodationRooms.GetSingleByConditionAsync(accommodationRoom =>
                accommodationRoom.AccommodationRoomId == accommodationRoomId);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (response1.Error is not null)
            {
                throw response1.Error;
            }

            logger.LogInformation("Calling AccommodationRoomRepository, method Delete");
            var response2 = unitOfWork.AccommodationRooms.Delete(response1.Entity!);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                throw response2.Error;
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting RoomService, DeleteAccommodationRoomAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<ModifyResponse<AccommodationRoom>> UpdateAccommodationRoom(AccommodationRoomToGetDto accommodationRoom)
    {
        var result = new ModifyResponse<AccommodationRoom>();

        try
        {
            logger.LogInformation("Entering RoomService, UpdateAccommodationRoom");

            logger.LogInformation("Calling AccommodationRoomRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.AccommodationRooms.GetSingleByConditionAsync(accommodationR =>
                accommodationR.AccommodationRoomId == accommodationRoom.AccommodationRoomId);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (response1.Error is not null)
            {
                throw response1.Error;
            }

            response1.Entity!.Quantity = accommodationRoom.Quantity;

            logger.LogInformation("Calling AccommodationRoomRepository, method Update");
            var response2 = unitOfWork.AccommodationRooms.Update(response1.Entity!);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method Update");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                throw response2.Error;
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting RoomService, UpdateAccommodationRoom");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }
}