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

public class PaymentRepository(RentContext context, IConfiguration configuration, ILogger<PaymentRepository> logger) : RepositoryBase<Payment>(context), IPaymentRepository
{
    public async Task<CreationResponse> CreateWithProcedure(PaymentToCreateDto payment)
    {
        logger.LogInformation("Entering PaymentRepository, method CreateWithProcedure");

        CreationResponse response = new();

        await using var connection = new SqlConnection(configuration["ConnectionStrings:RentDatabase"]);
        await connection.OpenAsync();

        var storedProcedureName = "sp_Payment_Insert";

        DynamicParameters parameters = new();
        parameters.Add("TenantId", payment.TenantId);
        parameters.Add("BillId", payment.BillId);
        parameters.Add("Amount", payment.Amount);
        try
        {
            logger.LogInformation("Querying 'sp_Payment_Insert' stored procedures");
            logger.LogInformation($"Parameters: @TenantId = {payment.TenantId}, @BillId = {payment.BillId}, @Amount = {payment.Amount}");
            var result = (await connection.QueryAsync(storedProcedureName, parameters,
                commandType: CommandType.StoredProcedure)).Select(entity => entity.PaymentId).FirstOrDefault();
            if (result != null) response.CreatedId = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting Payment entity: {ex.Message}");
            response.Error = ex;
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving PaymentRepository, method CreateWithProcedure");

        return response;
    }
}