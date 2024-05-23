using Dapper;
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

public class PaymentRepository(RentContext context, IConfiguration configuration, ILogger<PaymentRepository> logger) : RepositoryBase<Payment>(context), IPaymentRepository
{
    public async Task<Response<Guid>> CreateWithProcedure(PaymentToCreateDto payment)
    {
        logger.LogInformation("Entering PaymentRepository, method CreateWithProcedure");

        var response = new Response<Guid>();

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
            if (result != null) response.Body = result;
            logger.LogInformation("Queried stored procedure successfully");
        }
        catch (SqlException ex)
        {
            logger.LogInformation($"An error occured while inserting Payment entity: {ex.Message}");
            response.Exceptions.ToList().Add(ex);
        }
        await connection.CloseAsync();

        logger.LogInformation("Leaving PaymentRepository, method CreateWithProcedure");

        return response;
    }
}