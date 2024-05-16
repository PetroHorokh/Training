using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using Rent.DAL.RequestsAndResponses;
using Rent.Response.Library;

namespace Rent.BLL.Services;

public class RoomService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OwnerService> logger) : IRoomService
{
    public async Task<Response<IEnumerable<RoomToGetDto>>> GetAllRoomsAsync()
    {
        var result = new Response<IEnumerable<RoomToGetDto>>();

        try
        {
            logger.LogInformation("Entering RoomService, GetAllRoomsAsync");

            logger.LogInformation("Calling RoomRepository, method GetAllAsync");
            var response = await unitOfWork.Rooms.GetAllAsync();
            logger.LogInformation("Finished calling RoomRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Body.IsNullOrEmpty())
            {
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping rooms to RoomToGetDto");
            result.Body = response.Body!.Select(mapper.Map<RoomToGetDto>);

            logger.LogInformation("Exiting RoomService, GetAllRoomsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }

    public async Task<Response<IEnumerable<RoomToGetDto>>> GetPartialRoomsAsync(GetPartialRequest request)
    {
        var result = new Response<IEnumerable<RoomToGetDto>>();

        try
        {
            logger.LogInformation("Entering RoomService, GetPartialRoomsAsync");

            logger.LogInformation("Calling RoomRepository, method GetAllAsync");
            var response = await unitOfWork.Rooms.GetPartialAsync(request.Skip, request.Take);
            logger.LogInformation("Finished calling RoomRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping rooms to RoomToGetDto");
            result.Body = response.Body!.Select(mapper.Map<RoomToGetDto>);

            logger.LogInformation("Exiting RoomService, GetPartialRoomsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }

    public async Task<Response<IEnumerable<RoomTypeToGetDto>>> GetAllRoomTypesAsync()
    {
        var result = new Response<IEnumerable<RoomTypeToGetDto>>();

        try
        {
            logger.LogInformation("Entering RoomService, GetAllRoomTypesAsync");

            logger.LogInformation("Calling RoomTypeRepository, method GetAllAsync");
            var response = await unitOfWork.RoomTypes.GetAllAsync();
            logger.LogInformation("Finished calling RoomTypeRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping room types to RoomToGetDto");
            result.Body = response.Body!.Select(mapper.Map<RoomTypeToGetDto>);

            logger.LogInformation("Exiting RoomService, GetAllRoomTypesAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }

    public async Task<Response<IEnumerable<AccommodationToGetDto>>> GetAllAccommodationsAsync()
    {
        var result = new Response<IEnumerable<AccommodationToGetDto>>();

        try
        {
            logger.LogInformation("Entering RoomService, GetAllAccommodationsAsync");

            logger.LogInformation("Calling AccommodationRepository, method GetAllAsync");
            var response = await unitOfWork.Accommodations.GetAllAsync();
            logger.LogInformation("Finished calling AccommodationRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping accommodations to AccommodationToGetDto");
            result.Body = response.Body!.Select(mapper.Map<AccommodationToGetDto>);

            logger.LogInformation("Exiting RoomService, GetAllAccommodationsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }

    public async Task<Response<RoomToGetDto>> GetRoomByRoomIdAsync(Guid roomId)
    {
        var result = new Response<RoomToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetRoomByRoomIdAsync");

            logger.LogInformation("Calling RoomRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Rooms.GetSingleByConditionAsync(room => room.RoomId == roomId);
            logger.LogInformation("Finished calling RoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping room to RoomToGetDto");
            result.Body = mapper.Map<RoomToGetDto>(response.Body);

            logger.LogInformation("Exiting RoomService, GetRoomByRoomIdAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }

    public async Task<Response<RoomToGetDto>> GetRoomByNumberAsync(int roomNumber)
    {
        var result = new Response<RoomToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetRoomByNumberAsync");

            logger.LogInformation("Calling RoomRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Rooms.GetSingleByConditionAsync(room => room.Number == roomNumber);
            logger.LogInformation("Finished calling RoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping room to RoomToGetDto");
            result.Body = mapper.Map<RoomToGetDto>(response.Body);

            logger.LogInformation("Exiting RoomService, GetRoomByNumberAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }

    public async Task<Response<AccommodationRoomToGetDto>> GetAccommodationRoomByIdAsync(Guid accommodationRoomId)
    {
        var result = new Response<AccommodationRoomToGetDto>();

        try
        {
            logger.LogInformation("Entering RoomService, GetAccommodationRoomByIdAsync");

            logger.LogInformation("Calling AccommodationRoomRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.AccommodationRooms.GetSingleByConditionAsync(accommodationRoom =>
                accommodationRoom.AccommodationRoomId == accommodationRoomId);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping room accommodation to AccommodationRoomToGetDto");
            result.Body = mapper.Map<AccommodationRoomToGetDto>(response.Body);

            logger.LogInformation("Exiting RoomService, GetAccommodationRoomByIdAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }

    public async Task<Response<IEnumerable<AccommodationRoomToGetDto>>> GetAccommodationRoomsByRoomIdAsync(Guid roomId)
    {
        var result = new Response<IEnumerable<AccommodationRoomToGetDto>>();

        try
        {
            logger.LogInformation("Entering RoomService, GetAccommodationRoomsByRoomIdAsync");

            logger.LogInformation("Calling AccommodationRoomRepository, method GetByConditionAsync");
            var response =
                await unitOfWork.AccommodationRooms.GetByConditionAsync(accommodationRoom =>
                    accommodationRoom.RoomId == roomId);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping room accommodations to AccommodationRoomToGetDto");
            result.Body = response.Body!.Select(mapper.Map<AccommodationRoomToGetDto>);

            logger.LogInformation("Exiting RoomService, GetAccommodationRoomsByRoomIdAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }

    public async Task<Response<Guid>> CreateRoomAsync(RoomToCreateDto room)
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

    public async Task<Response<int>> CreateRoomTypeAsync(RoomTypeToCreateDto roomType)
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

    public async Task<Response<Guid>> CreateAccommodationRoomAsync(AccommodationRoomToCreateDto accommodationRoom)
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

    public async Task<Response<int>> CreateAccommodationAsync(AccommodationToCreateDto accommodation)
    {
        logger.LogInformation("Entering RoomService, CreateAccommodationAsync");

        logger.LogInformation("Calling AccommodationRepository, method CreateWithProcedure");
        logger.LogInformation($@"Parameters: @Name = {accommodation.Name}");
        var result = await unitOfWork.Accommodations.CreateWithProcedure(accommodation);
        logger.LogInformation("Finished calling AccommodationRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting RoomService, CreateAccommodationAsync");
        return result;
    }

    public async Task<Response<EntityEntry<Room>>> DeleteRoomAsync(Guid roomId)
    {
        var result = new Response<EntityEntry<Room>>();

        try
        {
            logger.LogInformation("Entering RoomService, DeleteRoomAsync");

            logger.LogInformation("Calling RoomRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.Rooms.GetSingleByConditionAsync(room => room.RoomId == roomId);
            logger.LogInformation("Finished calling RoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (!response1.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response1.Exceptions;
            }

            logger.LogInformation("Calling RoomRepository, method Delete");
            var response2 = unitOfWork.Rooms.Delete(response1.Body!);
            logger.LogInformation("Finished calling RoomRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (!response2.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response2.Exceptions;
            }

            await unitOfWork.SaveAsync();

            result.Body = response2.Body;

            logger.LogInformation("Exiting RoomService, DeleteRoomAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }

    public async Task<Response<EntityEntry<AccommodationRoom>>> DeleteAccommodationRoomAsync(Guid accommodationRoomId)
    {
        var result = new Response<EntityEntry<AccommodationRoom>>();

        try
        {
            logger.LogInformation("Entering RoomService, DeleteAccommodationRoomAsync");

            logger.LogInformation("Calling AccommodationRoomRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.AccommodationRooms.GetSingleByConditionAsync(accommodationRoom =>
                accommodationRoom.AccommodationRoomId == accommodationRoomId);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (!response1.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response1.Exceptions;
            }

            logger.LogInformation("Calling AccommodationRoomRepository, method Delete");
            var response2 = unitOfWork.AccommodationRooms.Delete(response1.Body!);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (!response2.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response2.Exceptions;
            }

            await unitOfWork.SaveAsync();

            result.Body = response2.Body;

            logger.LogInformation("Exiting RoomService, DeleteAccommodationRoomAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }

    public async Task<Response<EntityEntry<AccommodationRoom>>> UpdateAccommodationRoom(AccommodationRoomToGetDto accommodationRoom)
    {
        var result = new Response<EntityEntry<AccommodationRoom>>();

        try
        {
            logger.LogInformation("Entering RoomService, UpdateAccommodationRoom");

            logger.LogInformation("Calling AccommodationRoomRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.AccommodationRooms.GetSingleByConditionAsync(accommodationR =>
                accommodationR.AccommodationRoomId == accommodationRoom.AccommodationRoomId);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (!response1.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response1.Exceptions;
            }

            response1.Body!.Quantity = accommodationRoom.Quantity;

            logger.LogInformation("Calling AccommodationRoomRepository, method Update");
            var response2 = unitOfWork.AccommodationRooms.Update(response1.Body!);
            logger.LogInformation("Finished calling AccommodationRoomRepository, method Update");

            result.TimeStamp = response2.TimeStamp;
            if (!response2.Exceptions.IsNullOrEmpty())
            {
                result.Exceptions = response2.Exceptions;
            }

            await unitOfWork.SaveAsync();

            result.Body = response2.Body;

            logger.LogInformation("Exiting RoomService, UpdateAccommodationRoom");
            return result;
        }
        catch (Exception ex)
        {
            result.Exceptions.ToList().Add(ex);
            return result;
        }
    }
}