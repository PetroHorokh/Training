using Dapper;
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

namespace Rent.DAL.Repositories;

public class RentRepository(RentContext context, IConfiguration configuration, ILogger<RentRepository> logger) : RepositoryBase<Models.Rent>(context), IRentRepository
{
    public async Task<CreationResponse> CreateWithProcedure(RentToCreateDto rent)
    {
        logger.LogInformation("Entering RentRepository, method CreateWithProcedure");

        CreationResponse response = new();

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);
        await connection.OpenAsync();

        var storedProcedureName = "sp_Rent_Insert";

        DynamicParameters parameters = new();
        parameters.Add("AssetId", rent.AssetId);
        parameters.Add("TenantId", rent.TenantId);
        parameters.Add("StartDate", rent.StartDate);
        parameters.Add("EndDate", rent.EndDate);
        try
        {
            logger.LogInformation("Querying 'sp_Rent_Insert' stored procedures");
            logger.LogInformation(
                $"Parameters: @AssetId = {rent.AssetId}, @TenantId = {rent.TenantId}, @StartDate = {rent.StartDate}, @EndDate = {rent.EndDate}");
            var result = (await connection.QueryAsync(storedProcedureName, parameters,
                commandType: CommandType.StoredProcedure)).Select(entity => entity.RentId).FirstOrDefault();
            if (result != null) response.CreatedId = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting Rent entity: {ex.Message}");
            response.Error = ex;
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving RentRepository, method CreateWithProcedure");

        return response;
    }
}