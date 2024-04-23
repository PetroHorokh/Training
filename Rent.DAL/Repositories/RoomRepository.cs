using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rent.DAL.Context;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;
using System.Data;
using System.Net;
using Rent.DAL.RequestsAndResponses;

namespace Rent.DAL.Repositories;

public class RoomRepository(RentContext context, IConfiguration configuration, ILogger<RoomRepository> logger) : RepositoryBase<Room>(context), IRoomRepository
{
    public async Task<CreationResponse> CreateWithProcedure(RoomToCreateDto room)
    {
        logger.LogInformation("Entering RoomRepository, method CreateWithProcedure");

        CreationResponse response = new();

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);
        await connection.OpenAsync();

        var storedProcedureName = "sp_Room_Insert";

        DynamicParameters parameters = new ();
        parameters.Add("Number", room.Number);
        parameters.Add("AddressId", room.AddressId);
        parameters.Add("Area", room.Area);
        parameters.Add("RoomTypeId", room.RoomTypeId);
        try
        {
            logger.LogInformation("Querying 'sp_Room_Insert' stored procedures");
            logger.LogInformation($"Parameters: @Number = {room.Number}, @AddressId = {room.AddressId}, @Area = {room.Area}, @RoomTypeId = {room.RoomTypeId}");
            var result = (await connection.QueryAsync(storedProcedureName, parameters,
                commandType: CommandType.StoredProcedure)).Select(entity => entity.RoomId).FirstOrDefault();
            if (result != null) response.CreatedId = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting Room entity: {ex.Message}");
            response.Error = ex;
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving RoomRepository, method CreateWithProcedure");

        return response;
    }
}