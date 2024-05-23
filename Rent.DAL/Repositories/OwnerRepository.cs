using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;
using System.Data;
using Rent.DAL.Context;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories;

public class OwnerRepository(RentContext context, IConfiguration configuration, ILogger<OwnerRepository> logger) :RepositoryBase<Owner>(context), IOwnerRepository
{
    public async Task<Response<Guid>> CreateWithProcedure(OwnerToCreateDto owner)
    {
        logger.LogInformation("Entering OwnerRepository, method CreateWithProcedure");

        var response = new Response<Guid>();

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);
        await connection.OpenAsync();

        var storedProcedureName = "sp_Owner_Insert";

        DynamicParameters parameters = new();
        parameters.Add("Name", owner.Name);
        parameters.Add("UserId", owner.UserId);
        parameters.Add("AddressId", owner.AddressId);
        try
        {
            logger.LogInformation("Querying 'sp_Owner_Insert' stored procedures");
            logger.LogInformation($"Parameters: @Name = {owner.Name}, @UserId = {owner.UserId}, @AddressId = {owner.AddressId}");
            var result = (await connection.QueryAsync(storedProcedureName, parameters,
                commandType: CommandType.StoredProcedure)).Select(entity => entity.OwnerId).FirstOrDefault();
            if (result != null) response.Body = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting Owner entity: {ex.Message}");
            response.Exceptions.ToList().Add(ex);
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving OwnerRepository, method CreateWithProcedure");

        return response;
    }
}