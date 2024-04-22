using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rent.DAL.Context;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;
using Rent.DAL.Responses;
using System;
using System.Data;

namespace Rent.DAL.Repositories;

public class UserRepository(RentContext context, IConfiguration configuration, ILogger<UserRepository> logger) : RepositoryBase<User>(context), IUserRepository
{
    public async Task<CreationResponse> CreateWithProcedure(UserToCreateDto user)
    {
        logger.LogInformation("Entering UserRepository, method CreateWithProcedure");

        CreationResponse response = new();

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);
        await connection.OpenAsync();

        var storedProcedureName = "sp_User_Insert";

        DynamicParameters parameters = new();
        parameters.Add("Name", user.Name);
        parameters.Add("NormalizedName", user.Name.ToLower());
        parameters.Add("Password", user.Password);
        parameters.Add("Email", user.Email);
        parameters.Add("NormalizedEmail", user.Email.ToLower());
        parameters.Add("PhoneNumber", user.PhoneNumber);
        try
        {
            logger.LogInformation("Querying 'sp_User_Insert' stored procedures");
            logger.LogInformation($"Parameters: @Name = {user.Name}, @NormalizedName = {user.Name.ToLower()}, @Password = {user.Password}, @Email = {user.Email}, @NormalizedEmail = {user.Email.ToLower()}, @PhoneNumber = {user.PhoneNumber}");
            var result = (await connection.QueryAsync(storedProcedureName, parameters,
                commandType: CommandType.StoredProcedure)).Select(entity => entity.UserId).FirstOrDefault();
            if (result != null) response.CreatedId = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting User entity: {ex.Message}");
            response.Error = ex;
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving UserRepository, method CreateWithProcedure");

        return response;
    }
}