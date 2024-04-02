using System.Data.SqlTypes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Responses;
using Rent.DAL.UnitOfWork;

namespace Rent.BLL.Services;

public class TenantService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TenantService> logger) : ITenantService
{
    public async Task<IEnumerable<TenantToGetDto>> GetAllTenantsAsync()
    {
        logger.LogInformation("Entering TenantService, GetAllTenantsAsync");

        logger.LogInformation("Calling TenantRepository, method GetAllAsync");
        var tenants = (await unitOfWork.Tenants.GetAllAsync()).ToList();
        logger.LogInformation("Finished calling TenantRepository, method GetAllAsync");

        logger.LogInformation($"Mapping tenants to TenantToGetDto");
        var result = tenants.Select(mapper.Map<TenantToGetDto>);

        logger.LogInformation("Exiting TenantService, GetAllTenantsAsync");
        return result.ToList();
    }

    public async Task<IEnumerable<BillToGetDto>> GetAllBillsAsync()
    {
        logger.LogInformation("Entering TenantService, GetAllBillsAsync");

        logger.LogInformation("Calling BillRepository, method GetAllAsync");
        var bills = (await unitOfWork.Bills.GetAllAsync()).ToList();
        logger.LogInformation("Finished calling BillRepository, method GetAllAsync");

        logger.LogInformation($"Mapping bills to BillToGetDto");
        var result = bills.Select(mapper.Map<BillToGetDto>);

        logger.LogInformation("Exiting TenantService, GetAllBillsAsync");
        return result.ToList();
    }

    public async Task<IEnumerable<RentToGetDto>> GetAllRentsAsync()
    {
        logger.LogInformation("Entering TenantService, GetAllRentsAsync");

        logger.LogInformation("Calling RentRepository, method GetAllAsync");
        var rents = (await unitOfWork.Rents.GetAllAsync()).ToList();
        logger.LogInformation("Finished calling RentRepository, method GetAllAsync");

        logger.LogInformation($"Mapping rents to RentToGetDto");
        var result = rents.Select(mapper.Map<RentToGetDto>);

        logger.LogInformation("Exiting TenantService, GetAllRentsAsync");
        return result.ToList();
    }

    public async Task<TenantToGetDto?> GetTenantByIdAsync(Guid tenantId)
    {
        logger.LogInformation("Entering TenantService, GetTenantByIdAsync");

        logger.LogInformation("Calling TenantRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameter: tenantId = {tenantId}");
        var tenant = await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.TenantId == tenantId);
        logger.LogInformation("Finished calling TenantRepository, method GetSingleByConditionAsync");

        logger.LogInformation($"Mapping tenant to TenantToGetDto");
        var result = mapper.Map<TenantToGetDto>(tenant);

        logger.LogInformation("Exiting TenantService, GetTenantByIdAsync");
        return result;
    }

    public async Task<TenantToGetDto?> GetTenantByNameAsync(string tenantName)
    {
        logger.LogInformation("Entering TenantService, GetTenantByNameAsync");

        logger.LogInformation("Calling TenantRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameter: TenantName = {tenantName}");
        var tenant = await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.Name == tenantName);
        logger.LogInformation("Finished calling TenantRepository, method GetSingleByConditionAsync");

        logger.LogInformation($"Mapping tenant to TenantToGetDto");
        var result = mapper.Map<TenantToGetDto>(tenant);

        logger.LogInformation("Exiting TenantService, GetTenantByNameAsync");
        return result;
    }

    public async Task<AddressToGetDto?> GetTenantAddressAsync(Guid tenantId)
    {
        logger.LogInformation("Entering TenantService, GetTenantAddressAsync");

        logger.LogInformation("Calling TenantRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameter: TenantId = {tenantId}");
        var address = (await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.TenantId == tenantId,
            tenant => tenant.Address!))!.Address;
        logger.LogInformation("Finished calling TenantRepository, method GetSingleByConditionAsync");

        logger.LogInformation($"Mapping address to AddressToGetDto");
        var result = mapper.Map<AddressToGetDto>(address);

        logger.LogInformation("Exiting TenantService, GetTenantAddressAsync");
        return result;
    }

    public async Task<IEnumerable<RentToGetDto>> GetTenantRentsAsync(Guid tenantId)
    {
        logger.LogInformation("Entering TenantService, GetTenantRentsAsync");

        logger.LogInformation("Calling RentRepository, method GetByConditionAsync");
        logger.LogInformation($"Parameter: TenantId = {tenantId}");
        var rents = (await unitOfWork.Rents.GetByConditionAsync(rent => rent.TenantId == tenantId));
        logger.LogInformation("Finished calling RentRepository, method GetByConditionAsync");

        logger.LogInformation($"Mapping rents to RentToGetDto");
        var result = rents.Select(mapper.Map<RentToGetDto>);

        logger.LogInformation("Exiting TenantService, GetTenantRentsAsync");
        return result;
    }

    public async Task<IEnumerable<BillToGetDto>> GetTenantBillsAsync(Guid tenantId)
    {
        logger.LogInformation("Entering TenantService, GetTenantBillsAsync");

        logger.LogInformation("Calling BillRepository, method GetByConditionAsync");
        logger.LogInformation($"Parameter: TenantId = {tenantId}");
        var bills = (await unitOfWork.Bills.GetByConditionAsync(bill => bill.TenantId == tenantId));
        logger.LogInformation("Finished calling BillRepository, method GetByConditionAsync");

        logger.LogInformation($"Mapping bills to BillToGetDto");
        var result = bills.Select(mapper.Map<BillToGetDto>);

        logger.LogInformation("Exiting TenantService, GetTenantBillsAsync");
        return result;
    }

    public async Task<IEnumerable<BillToGetDto>> GetTenantPaymentsAsync(Guid tenantId)
    {
        logger.LogInformation("Entering TenantService, GetTenantPaymentsAsync");

        logger.LogInformation("Calling PaymentRepository, method GetByConditionAsync");
        logger.LogInformation($"Parameter: TenantId = {tenantId}");
        var bills = (await unitOfWork.Payments.GetByConditionAsync(payment => payment.TenantId == tenantId));
        logger.LogInformation("Finished calling PaymentRepository, method GetByConditionAsync");

        logger.LogInformation($"Mapping bills to BillToGetDto");
        var result = bills.Select(mapper.Map<BillToGetDto>);

        logger.LogInformation("Exiting TenantService, GetTenantPaymentsAsync");
        return result;
    }

    public async Task<CreationResponse> CreateTenantAsync(TenantToCreateDto tenant)
    {
        logger.LogInformation("Entering TenantService, CreateTenantAsync");

        logger.LogInformation("Calling TenantRepository, method CreateWithProcedure");
        logger.LogInformation($"Parameters: @Name = {tenant.Name}, @AddressId = {tenant.AddressId}, @Description = {tenant.Description}, @BankName = {tenant.BankName}, @Director = {tenant.Director}");
        var result = await unitOfWork.Tenants.CreateWithProcedure(tenant);
        logger.LogInformation("Finished calling TenantRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting TenantService, CreateTenantAsync");
        return result;
    }

    public async Task<CreationResponse> CreateRentAsync(RentToCreateDto rent)
    {
        logger.LogInformation("Entering TenantService, CreateRentAsync");

        logger.LogInformation("Calling RentRepository, method CreateWithProcedure");
        logger.LogInformation(
            $"Parameters: @AssetId = {rent.AssetId}, @TenantId = {rent.TenantId}, @StartDate = {rent.StartDate}, @EndDate = {rent.EndDate}");
        var result = await unitOfWork.Rents.CreateWithProcedure(rent);
        logger.LogInformation("Finished calling RentRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting TenantService, CreateRentAsync");
        return result;
    }

    public async Task<UpdatingResponse> DeleteTenantAsync(Guid tenantId)
    {
        logger.LogInformation("Entering TenantService, DeleteTenantAsync");
        Exception? error = null;

        logger.LogInformation("Calling RoomRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameter: TenantId = {tenantId}");
        var tenant = await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.TenantId == tenantId);
        logger.LogInformation("Finished calling RoomRepository, method GetSingleByConditionAsync");
        try
        {
            if (tenant != null)
            {
                logger.LogInformation("Calling RoomRepository, method Delete");
                unitOfWork.Tenants.Delete(tenant);
                logger.LogInformation("Finished calling RoomRepository, method Delete");

                await unitOfWork.SaveAsync();
            }
            else
            {
                throw new SqlNullValueException("Couldn't find tenant");
            }
        }
        catch (DbUpdateException ex)
        {
            logger.LogInformation($"An error occured while deleting Tenant entity: {ex.InnerException}");
            error = ex;
        }
        catch (SqlNullValueException ex)
        {
            logger.LogInformation($"An error occured while deleting Tenant entity: {ex.InnerException}");
            error = ex;
        }

        logger.LogInformation("Exiting TenantService, DeleteTenantAsync");
        return new UpdatingResponse() { DateTime = DateTime.Now, Error = error };
    }

    public async Task<UpdatingResponse> UpdateTenantAsync(TenantToUpdateDto newTenant)
    {
        logger.LogInformation("Entering TenantService, UpdateTenantAsync");

        Exception? error = null;

        logger.LogInformation("Calling TenantRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameters: AddressId = {newTenant.AddressId}, Name = {newTenant.Name}, BankName = {newTenant.BankName}, Director = {newTenant.Director}, Description = {newTenant.Description}");
        var tenant =
            await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.TenantId == newTenant.TenantId);
        logger.LogInformation("Finished calling TenantRepository, method GetSingleByConditionAsync");

        try
        {
            if (tenant != null)
            {
                tenant.Name = newTenant.Name;
                tenant.BankName = newTenant.BankName;
                tenant.AddressId = newTenant.AddressId;
                tenant.Director = newTenant.Director;
                tenant.Description = newTenant.Description;

                logger.LogInformation("Calling TenantRepository, method Update");
                unitOfWork.Tenants.Update(tenant);
                logger.LogInformation("Finished calling TenantRepository, method Update");

                await unitOfWork.SaveAsync();
            }
            else
            {
                throw new SqlNullValueException("Couldn't find tenant");
            }
        }
        catch (DbUpdateException ex)
        {
            logger.LogInformation($"An error occured while deleting Tenant entity: {ex.InnerException}");
            error = ex;
        }
        catch (SqlNullValueException ex)
        {
            logger.LogInformation($"An error occured while deleting Tenant entity: {ex.InnerException}");
            error = ex;
        }

        logger.LogInformation("Exiting TenantService, UpdateTenantAsync");
        return new UpdatingResponse() { DateTime = DateTime.Now, Error = error };
    }

    public async Task<UpdatingResponse> CancelRentAsync(Guid rentId)
    {
        logger.LogInformation("Entering TenantService, CancelRentAsync");

        Exception? error = null;

        logger.LogInformation("Calling RentRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameters: RentId = {rentId}");
        var rent = await unitOfWork.Rents.GetSingleByConditionAsync(rent => rent.RentId == rentId);
        logger.LogInformation("Finished calling RentRepository, method GetSingleByConditionAsync");

        try
        {
            if (rent != null)
            {
                rent.EndDate = DateTime.Now;

                logger.LogInformation("Calling RentRepository, method Update");
                unitOfWork.Rents.Update(rent);
                logger.LogInformation("Finished calling RentRepository, method Update");

                await unitOfWork.SaveAsync();
            }
            else
            {
                throw new SqlNullValueException("Couldn't find rent");
            }
        }
        catch (DbUpdateException ex)
        {
            logger.LogInformation($"An error occured while updating Rent entity: {ex.InnerException}");
            error = ex;
        }
        catch (SqlNullValueException ex)
        {
            logger.LogInformation($"An error occured while updating Rent entity: {ex.InnerException}");
            error = ex;
        }

        logger.LogInformation("Exiting TenantService, CancelRentAsync");
        return new UpdatingResponse() { DateTime = DateTime.Now, Error = error };
    }

    public async Task<CreationResponse> CreatePaymentAsync(PaymentToCreateDto payment)
    {
        logger.LogInformation("Entering TenantService, CreatePaymentAsync");

        logger.LogInformation("Calling PaymentRepository, method CreateWithProcedure");
        logger.LogInformation($"Parameters: @TenantId = {payment.TenantId}, @BillId = {payment.BillId}, @Amount = {payment.Amount}");
        var result = await unitOfWork.Payments.CreateWithProcedure(payment);
        logger.LogInformation("Finished calling PaymentRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting TenantService, CreatePaymentAsync");
        return result;
    }
}