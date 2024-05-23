﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

public class AssetRepository(RentContext context, IConfiguration configuration, ILogger<RoomRepository> logger) : RepositoryBase<Asset>(context), IAssetRepository
{
    public async Task<Response<Guid>> CreateWithProcedure(AssetToCreateDto asset)
    {
        logger.LogInformation("Entering AssetRepository, method CreateWithProcedure");

        var response = new Response<Guid>();

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);
        await connection.OpenAsync();

        var storedProcedureName = "sp_Asset_Insert";

        DynamicParameters parameters = new();
        parameters.Add("OwnerId", asset.OwnerId);
        parameters.Add("RoomId", asset.RoomId);
        try
        {
            logger.LogInformation("Querying 'sp_Owner_Insert' stored procedures");
            logger.LogInformation($"Parameters: @OwnerId = {asset.OwnerId}, @RoomId = {asset.RoomId}");
            var result = (await connection.QueryAsync(storedProcedureName, parameters,
                commandType: CommandType.StoredProcedure)).Select(entity => entity.AssetId).FirstOrDefault();
            if (result != null) response.Body = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting Asset entity: {ex.Message}");
            response.Exceptions.ToList().Add(ex);
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving AssetRepository, method CreateWithProcedure");

        return response;
    }
}