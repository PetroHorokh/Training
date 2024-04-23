﻿using System.Data.SqlTypes;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RequestsAndResponses;
using Rent.DAL.UnitOfWork;

namespace Rent.BLL.Services;

public class TenantService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TenantService> logger) : ITenantService
{
    public async Task<GetMultipleResponse<TenantToGetDto>> GetAllTenantsAsync()
    {
        var result = new GetMultipleResponse<TenantToGetDto>();

        try
        {
            logger.LogInformation("Entering TenantService, GetAllTenantsAsync");

            logger.LogInformation("Calling TenantRepository, method GetAllAsync");
            var response = await unitOfWork.Tenants.GetAllAsync();
            logger.LogInformation("Finished calling TenantRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping tenants to TenantToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<TenantToGetDto>);

            logger.LogInformation("Exiting TenantService, GetAllTenantsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<TenantToGetDto>> GetTenantsPartialAsync(GetPartialRequest request)
    {
        var result = new GetMultipleResponse<TenantToGetDto>();

        try
        {
            logger.LogInformation("Entering TenantService, GetTenantsPartialAsync");

            logger.LogInformation("Calling TenantRepository, method GetPartialAsync");
            var response = await unitOfWork.Tenants.GetPartialAsync(request.Skip, request.Take);
            logger.LogInformation("Finished calling TenantRepository, method GetPartialAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping tenants to TenantToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<TenantToGetDto>);

            logger.LogInformation("Exiting TenantService, GetTenantsPartialAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<BillToGetDto>> GetAllBillsAsync()
    {
        var result = new GetMultipleResponse<BillToGetDto>();

        try
        {
            logger.LogInformation("Entering TenantService, GetAllBillsAsync");

            logger.LogInformation("Calling BillRepository, method GetAllAsync");
            var response = await unitOfWork.Bills.GetAllAsync();
            logger.LogInformation("Finished calling BillRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping bills to BillToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<BillToGetDto>);

            logger.LogInformation("Exiting TenantService, GetAllBillsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<RentToGetDto>> GetAllRentsAsync()
    {
        var result = new GetMultipleResponse<RentToGetDto>();

        try
        {
            logger.LogInformation("Entering TenantService, GetAllRentsAsync");

            logger.LogInformation("Calling RentRepository, method GetAllAsync");
            var response = await unitOfWork.Rents.GetAllAsync();
            logger.LogInformation("Finished calling RentRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping rents to RentToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<RentToGetDto>);

            logger.LogInformation("Exiting TenantService, GetAllRentsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetSingleResponse<TenantToGetDto>> GetTenantByIdAsync(Guid tenantId)
    {
        var result = new GetSingleResponse<TenantToGetDto>();

        try
        {
            logger.LogInformation("Entering TenantService, GetTenantByIdAsync");

            logger.LogInformation("Calling TenantRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.TenantId == tenantId);
            logger.LogInformation("Finished calling TenantRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping tenant to TenantToGetDto");
            result.Entity = mapper.Map<TenantToGetDto>(response.Entity);

            logger.LogInformation("Exiting TenantService, GetTenantByIdAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<RentToGetDto>> GetTenantRentsAsync(Guid tenantId)
    {
        var result = new GetMultipleResponse<RentToGetDto>();

        try
        {
            logger.LogInformation("Entering TenantService, GetTenantRentsAsync");

            logger.LogInformation("Calling RentRepository, method GetByConditionAsync");
            var response = await unitOfWork.Rents.GetByConditionAsync(rent => rent.TenantId == tenantId);
            logger.LogInformation("Finished calling RentRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping rents to RentToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<RentToGetDto>);

            logger.LogInformation("Exiting TenantService, GetTenantRentsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<BillToGetDto>> GetTenantBillsAsync(Guid tenantId)
    {
        var result = new GetMultipleResponse<BillToGetDto>();

        try
        {
            logger.LogInformation("Entering TenantService, GetTenantBillsAsync");

            logger.LogInformation("Calling BillRepository, method GetByConditionAsync");
            var response = await unitOfWork.Bills.GetByConditionAsync(bill => bill.TenantId == tenantId);
            logger.LogInformation("Finished calling BillRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping bills to BillToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<BillToGetDto>);

            logger.LogInformation("Exiting TenantService, GetTenantBillsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<PaymentToGetDto>> GetTenantPaymentsAsync(Guid tenantId)
    {
        var result = new GetMultipleResponse<PaymentToGetDto>();

        try
        {
            logger.LogInformation("Entering TenantService, GetTenantPaymentsAsync");

            logger.LogInformation("Calling PaymentRepository, method GetByConditionAsync");
            var response = await unitOfWork.Payments.GetByConditionAsync(payment => payment.TenantId == tenantId);
            logger.LogInformation("Finished calling PaymentRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping payments to PaymentToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<PaymentToGetDto>);

            logger.LogInformation("Exiting TenantService, GetTenantPaymentsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
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

    public async Task<ModifyResponse<Tenant>> DeleteTenantAsync(Guid tenantId)
    {
        var result = new ModifyResponse<Tenant>();

        try
        {
            logger.LogInformation("Entering TenantService, DeleteTenantAsync");

            logger.LogInformation("Calling TenantRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.TenantId == tenantId);
            logger.LogInformation("Finished calling TenantRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (response1.Error is not null)
            {
                throw response1.Error;
            }

            logger.LogInformation("Calling TenantRepository, method Delete");
            var response2 = unitOfWork.Tenants.Delete(response1.Entity!);
            logger.LogInformation("Finished calling TenantRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                throw response2.Error;
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting TenantService, DeleteTenantAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<ModifyResponse<Tenant>> UpdateTenantAsync(TenantToGetDto newTenant)
    {
        var result = new ModifyResponse<Tenant>();

        try
        {
            logger.LogInformation("Entering TenantService, UpdateTenantAsync");

            logger.LogInformation("Calling TenantRepository, method GetSingleByConditionAsync");
            var response1 =
                await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.TenantId == newTenant.TenantId);
            logger.LogInformation("Finished calling TenantRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (response1.Error is not null)
            {
                throw response1.Error;
            }

            response1.Entity!.Name = newTenant.Name;
            response1.Entity!.Director = newTenant.Director;
            response1.Entity!.Description = newTenant.Description;
            response1.Entity!.BankName = newTenant.BankName;
            response1.Entity!.AddressId = newTenant.AddressId;

            logger.LogInformation("Calling TenantRepository, method Update");
            var response2 = unitOfWork.Tenants.Update(response1.Entity!);
            logger.LogInformation("Finished calling TenantRepository, method Update");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                throw response2.Error;
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting TenantService, UpdateTenantAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<ModifyResponse<DAL.Models.Rent>> CancelRentAsync(Guid rentId)
    {
        var result = new ModifyResponse<DAL.Models.Rent>();

        try
        {
            logger.LogInformation("Entering TenantService, CancelRentAsync");

            logger.LogInformation("Calling RentRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.Rents.GetSingleByConditionAsync(rent => rent.RentId == rentId);
            logger.LogInformation("Finished calling RentRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (response1.Error is not null)
            {
                throw response1.Error;
            }

            response1.Entity!.EndDate = DateTime.Now;

            logger.LogInformation("Calling RentRepository, method Update");
            var response2 = unitOfWork.Rents.Update(response1.Entity!);
            logger.LogInformation("Finished calling RentRepository, method Update");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                throw response2.Error;
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting TenantService, CancelRentAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
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