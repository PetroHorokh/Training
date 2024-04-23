using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;
using System;
using System.Net;
using Dapper;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using Microsoft.Extensions.Logging;
using Rent.DAL.Context;
using Rent.DAL.RequestsAndResponses;

namespace Rent.DAL.Repositories;

public class TenantRepository(RentContext context, IConfiguration configuration, ILogger<TenantRepository> logger) : RepositoryBase<Tenant>(context), ITenantRepository
{
    public async Task<CreationResponse> CreateWithProcedure(TenantToCreateDto tenant)
    {
        logger.LogInformation("Entering TenantRepository, method CreateWithProcedure");

        CreationResponse response = new();

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);
        await connection.OpenAsync();

        var storedProcedureName = "sp_Tenant_Insert";

        DynamicParameters parameters = new();
        parameters.Add("Name", tenant.Name);
        parameters.Add("UserID",tenant.UserId);
        parameters.Add("AddressId", tenant.AddressId);
        parameters.Add("Description", tenant.Description);
        parameters.Add("BankName", tenant.BankName);
        parameters.Add("Director", tenant.Director);
        try
        {
            logger.LogInformation("Querying 'sp_Tenant_Insert' stored procedures");
            logger.LogInformation(
                $"Parameters: @Name = {tenant.Name}, @UserId = {tenant.UserId}, @AddressId = {tenant.AddressId}, @Description = {tenant.Description}, @BankName = {tenant.BankName}, @Director = {tenant.Director}");
            var result = (await connection.QueryAsync(storedProcedureName, parameters,
                commandType: CommandType.StoredProcedure)).Select(entity => entity.TenantId).FirstOrDefault();
            if (result != null) response.CreatedId = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting tenant entity: {ex.Message}");
            response.Error = ex;
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving TenantRepository, method CreateWithProcedure");

        return response;
    }
}