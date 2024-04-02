﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;
using Rent.DAL.Responses;
using System.Data;
using System.Net;

namespace Rent.DAL.Repositories;

public class RoomTypeRepository(RentContext context, IConfiguration configuration, ILogger<RoomTypeRepository> logger) : RepositoryBase<RoomType>(context), IRoomTypeRepository
{
    public async Task<CreationDictionaryResponse> CreateWithProcedure(RoomTypeToCreateDto roomType)
    {
        logger.LogInformation("Entering RoomTypeRepository, method CreateWithProcedure");

        CreationDictionaryResponse response = new();

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);
        await connection.OpenAsync();

        var storedProcedureName = "sp_RoomType_Insert";

        DynamicParameters parameters = new();
        parameters.Add("Name", roomType.Name);
        try
        {
            logger.LogInformation("Querying 'sp_RoomType_Insert' stored procedures");
            logger.LogInformation($"Parameters: @Number = {roomType.Name}");
            var result = (await connection.QueryAsync(storedProcedureName, parameters,
                commandType: CommandType.StoredProcedure)).Select(entity => entity.RoomTypeId).FirstOrDefault();
            if (result != null) response.CreatedId = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting RoomType entity: {ex.Message}");
            response.Error = ex;
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving RoomTypeRepository, method CreateWithProcedure");

        return response;
    }
}