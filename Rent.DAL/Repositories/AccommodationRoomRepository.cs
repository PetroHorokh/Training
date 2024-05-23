﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rent.DAL.Context;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;
using System.Data;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories;

public class AccommodationRoomRepository(RentContext context, IConfiguration configuration, ILogger<AccommodationRoomRepository> logger) : RepositoryBase<AccommodationRoom>(context), IAccommodationRoomRepository
{
    public async Task<Response<Guid>> CreateWithProcedure(AccommodationRoomToCreateDto accommodationRoom)
    {
        logger.LogInformation("Entering AccommodationRoomRepository, method CreateWithProcedure");

        var response = new Response<Guid>();

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);
        await connection.OpenAsync();

        var storedProcedureName = "sp_AccommodationRoom_Insert";

        DynamicParameters parameters = new();
        parameters.Add("AccommodationId", accommodationRoom.AccommodationId);
        parameters.Add("RoomId", accommodationRoom.RoomId);
        parameters.Add("Quantity", accommodationRoom.Quantity);
        try
        {
            logger.LogInformation("Querying 'sp_AccommodationRoom_Insert' stored procedures");
            logger.LogInformation(
                $@"Parameters: @AccommodationId = {accommodationRoom.AccommodationId}, @RoomId = {accommodationRoom.RoomId}, @Quantity = {accommodationRoom.Quantity}");
            var result = (await connection.QueryAsync(storedProcedureName, parameters,
                commandType: CommandType.StoredProcedure)).Select(entity => entity.AccommodationRoomId).FirstOrDefault();
            if (result != null) response.Body = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting AccommodationRoom entity: {ex.Message}");
            response.Exceptions.ToList().Add(ex);
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving AccommodationRoomRepository, method CreateWithProcedure");

        return response;
    }
}