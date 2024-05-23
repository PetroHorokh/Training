using Dapper;
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

public class AccommodationRepository(RentContext context, IConfiguration configuration, ILogger<AccommodationRepository> logger) : RepositoryBase<Accommodation>(context), IAccommodationRepository
{
    public async Task<Response<int>> CreateWithProcedure(AccommodationToCreateDto accommodation)
    {
        logger.LogInformation("Entering AccommodationRepository, method CreateWithProcedure");

        var response = new Response<int>();

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);
        await connection.OpenAsync();

        var storedProcedureName = "sp_Accommodation_Insert";

        DynamicParameters parameters = new();
        parameters.Add("Name", accommodation.Name);
        try
        {
            logger.LogInformation("Querying 'sp_Accommodation_Insert' stored procedures");
            logger.LogInformation($@"Parameters: @Name = {accommodation.Name}");
            var result = (await connection.QueryAsync(storedProcedureName, parameters,
                commandType: CommandType.StoredProcedure)).Select(entity => entity.AccommodationId).FirstOrDefault();
            if (result != null) response.Body = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting Accommodation entity: {ex.Message}");
            response.Exceptions.ToList().Add(ex);
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving AccommodationRepository, method CreateWithProcedure");

        return response;
    }
}