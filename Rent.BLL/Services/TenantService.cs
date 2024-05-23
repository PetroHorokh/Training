using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Rent.BLL.Services.Contracts;
using Rent.DAL.UnitOfWork;
using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rent.DTOs.Library;
using Rent.ExceptionLibrary;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.BLL.Services;

/// <summary>
/// Service for working with tenants
/// </summary>
/// <param name="unitOfWork">Parameter to use UnitOfWork pattern implemented in Dal layer</param>
/// <param name="mapper">Parameter to use mapper with provided profiles</param>
/// <param name="logger">Parameter to use logging</param>
/// <param name="memoryCache">Parameter to use in-memory caching</param>
public class TenantService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<TenantService> logger,
    IMemoryCache memoryCache) : ITenantService
{
    /// <summary>
    /// Property for setting root cache key for signalizing in-memory cache is used by current service
    /// </summary>
    private const string RootCacheKey = "Tenants";

    /// <summary>
    /// Property for setting SlidingExpiration after which caching of specific entity is being prolonged
    /// </summary>
    private const int SlidingExpiration = 2;

    /// <summary>
    /// Property for setting AbsoluteExpiration which is upper limit for caching specific entity
    /// </summary>
    private const int AbsoluteExpiration = 10;

    /// <summary>
    /// Get all tenants from UnitOfWork Tenants with method GetAllAsync
    /// </summary>
    /// <param name="includes">Parameter to use include with EF to add necessary related tables</param>
    /// <exception cref="ProcessException">Exception thrown when error occured while making a request to database</exception>
    /// <exception cref="AutoMapperMappingException">Exception thrown when error occured while mapping entities</exception>
    /// <returns>Returns <see cref="Response{IEnumerable}"/> of <see cref="TenantToGetDto"/> entity with either IEnumerable of <see cref="TenantToGetDto"/> entities or thrown exception</returns>
    public async Task<Response<IEnumerable<TenantToGetDto>>> GetAllTenantsAsync(
        params string[] includes)
    {
        var result = new Response<IEnumerable<TenantToGetDto>>();
        var cacheKey = RootCacheKey + "All";

        try
        {
            logger.LogInformation("Entering TenantService, GetAllTenantsAsync");

            logger.LogInformation("Checking memory cache for tenants");
            if (!memoryCache.TryGetValue(cacheKey, out List<TenantToGetDto>? tenants))
            {
                logger.LogInformation("Calling TenantRepository, method GetAllAsync");
                var response = await unitOfWork.Tenants.GetAllAsync(includes);
                logger.LogInformation("Finished calling TenantRepository, method GetAllAsync");


                if (!response.Exceptions.IsNullOrEmpty())
                {
                    logger.LogError("Exception occured while retrieving all tenants from database.");
                    result.Exceptions = response.Exceptions;
                }

                if (response.Body.IsNullOrEmpty())
                    tenants = new List<TenantToGetDto>();
                else
                {
                    logger.LogInformation($"Mapping tenants to TenantToGetDto");
                    tenants = response.Body!.Select(mapper.Map<TenantToGetDto>).ToList();
                }

                logger.LogInformation(
                    $"Adding tenants into memory cache with SlidingExpiration - {SlidingExpiration}; AbsoluteExpiration - {AbsoluteExpiration}");
                memoryCache.Set(cacheKey, tenants,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(SlidingExpiration))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(AbsoluteExpiration)));

                result.Body = tenants;
            }
            else
            {
                result.Body = tenants;
            }

            logger.LogInformation("Exiting TenantService, GetAllTenantsAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving all tenants from database.", ex));
        }

        return result;
    }

    public async Task<Response<IEnumerable<TenantToGetDto>>> GetTenantsPartialAsync(GetPartialRequest request,
        params string[] includes)
    {
        var result = new Response<IEnumerable<TenantToGetDto>>();

        try
        {
            logger.LogInformation("Entering TenantService, GetTenantsPartialAsync");

            logger.LogInformation("Calling TenantRepository, method GetPartialAsync");
            var response = await unitOfWork.Tenants.GetPartialAsync(request.Skip, request.Take, includes);
            logger.LogInformation("Finished calling TenantRepository, method GetPartialAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving partial tenants from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping tenants to TenantToGetDto");
            result.Body = response.Body!.Select(mapper.Map<TenantToGetDto>);

            logger.LogInformation("Exiting TenantService, GetTenantsPartialAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving partial tenants from database.", ex));
        }

        return result;
    }

    public async Task<Response<IEnumerable<TenantToGetDto>>> GetFilterTenantsAsync(GetFilteredRequest filter,
        params string[] includes)
    {
        var result = new Response<IEnumerable<TenantToGetDto>>();

        try
        {
            logger.LogInformation("Entering TenantService, GetFilterTenantsAsync");

            logger.LogInformation("Calling TenantRepository, method GetByConditionAsync");
            var response = await unitOfWork.Tenants.GetByConditionAsync(tenant =>
                tenant.Address.City.ToLower().Contains(filter.Filter)
                || tenant.Address.Street.ToLower().Contains(filter.Filter)
                || tenant.Address.Building.ToLower().Contains(filter.Filter)
                || tenant.Name.ToLower().Contains(filter.Filter)
                || tenant.Director.ToLower().Contains(filter.Filter)
                || tenant.BankName.ToLower().Contains(filter.Filter)
                || tenant.Description.ToLower().Contains(filter.Filter), includes);
            logger.LogInformation("Finished calling TenantRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving filtered tenants from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping tenants to TenantToGetDto");
            result.Body = response.Body!.Select(mapper.Map<TenantToGetDto>);

            logger.LogInformation("Exiting TenantService, GetFilterTenantsAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving filtered tenants from database.", ex));
        }

        return result;
    }

    public async Task<Response<IEnumerable<BillToGetDto>>> GetAllBillsAsync(
        params string[] includes)
    {
        var result = new Response<IEnumerable<BillToGetDto>>();

        try
        {
            logger.LogInformation("Entering TenantService, GetAllBillsAsync");

            logger.LogInformation("Calling BillRepository, method GetAllAsync");
            var response = await unitOfWork.Bills.GetAllAsync(includes);
            logger.LogInformation("Finished calling BillRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving all bills from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping bills to BillToGetDto");
            result.Body = response.Body!.Select(mapper.Map<BillToGetDto>);

            logger.LogInformation("Exiting TenantService, GetAllBillsAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving all bills from database.", ex));
        }

        return result;
    }

    public async Task<Response<IEnumerable<RentToGetDto>>> GetAllRentsAsync(
        params string[] includes)
    {
        var result = new Response<IEnumerable<RentToGetDto>>();

        try
        {
            logger.LogInformation("Entering TenantService, GetAllRentsAsync");

            logger.LogInformation("Calling RentRepository, method GetAllAsync");
            var response = await unitOfWork.Rents.GetAllAsync(includes);
            logger.LogInformation("Finished calling RentRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving all rents from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping rents to RentToGetDto");
            result.Body = response.Body!.Select(mapper.Map<RentToGetDto>);

            logger.LogInformation("Exiting TenantService, GetAllRentsAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving all rents from database.", ex));
        }

        return result;
    }

    public async Task<Response<TenantToGetDto>> GetTenantByIdAsync(Guid tenantId,
        params string[] includes)
    {
        var result = new Response<TenantToGetDto>();

        try
        {
            logger.LogInformation("Entering TenantService, GetTenantByIdAsync");

            logger.LogInformation("Calling TenantRepository, method GetSingleByConditionAsync");
            var response =
                await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.TenantId == tenantId, includes);
            logger.LogInformation("Finished calling TenantRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving tenant by id from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping tenant to TenantToGetDto");
            result.Body = mapper.Map<TenantToGetDto>(response.Body);

            logger.LogInformation("Exiting TenantService, GetTenantByIdAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving tenant by id from database.", ex));
        }

        return result;
    }

    public async Task<Response<IEnumerable<RentToGetDto>>> GetTenantRentsAsync(Guid tenantId,
        params string[] includes)
    {
        var result = new Response<IEnumerable<RentToGetDto>>();

        try
        {
            logger.LogInformation("Entering TenantService, GetTenantRentsAsync");

            logger.LogInformation("Calling RentRepository, method GetByConditionAsync");
            var response = await unitOfWork.Rents.GetByConditionAsync(rent => rent.TenantId == tenantId, includes);
            logger.LogInformation("Finished calling RentRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving all tenant's rents from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping rents to RentToGetDto");
            result.Body = response.Body!.Select(mapper.Map<RentToGetDto>);

            logger.LogInformation("Exiting TenantService, GetTenantRentsAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving all tenant's rents from database.", ex));
        }

        return result;
    }

    public async Task<Response<IEnumerable<BillToGetDto>>> GetTenantBillsAsync(Guid tenantId,
        params string[] includes)
    {
        var result = new Response<IEnumerable<BillToGetDto>>();

        try
        {
            logger.LogInformation("Entering TenantService, GetTenantBillsAsync");

            logger.LogInformation("Calling BillRepository, method GetByConditionAsync");
            var response = await unitOfWork.Bills.GetByConditionAsync(bill => bill.TenantId == tenantId, includes);
            logger.LogInformation("Finished calling BillRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving all tenant's bills from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping bills to BillToGetDto");
            result.Body = response.Body!.Select(mapper.Map<BillToGetDto>);

            logger.LogInformation("Exiting TenantService, GetTenantBillsAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving all tenant's bills from database.", ex));
        }

        return result;
    }

    public async Task<Response<IEnumerable<PaymentToGetDto>>> GetTenantPaymentsAsync(Guid tenantId,
        params string[] includes)
    {
        var result = new Response<IEnumerable<PaymentToGetDto>>();

        try
        {
            logger.LogInformation("Entering TenantService, GetTenantPaymentsAsync");

            logger.LogInformation("Calling PaymentRepository, method GetByConditionAsync");
            var response =
                await unitOfWork.Payments.GetByConditionAsync(payment => payment.TenantId == tenantId, includes);
            logger.LogInformation("Finished calling PaymentRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving all tenant's payments from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping payments to PaymentToGetDto");
            result.Body = response.Body!.Select(mapper.Map<PaymentToGetDto>);

            logger.LogInformation("Exiting TenantService, GetTenantPaymentsAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving all tenant's payments from database.", ex));
        }

        return result;
    }

    public async Task<Response<Guid>> CreateTenantAsync(TenantToCreateDto tenant)
    {
        logger.LogInformation("Entering TenantService, CreateTenantAsync");

        logger.LogInformation("Calling TenantRepository, method CreateWithProcedure");
        logger.LogInformation(
            $"Parameters: @Name = {tenant.Name}, @AddressId = {tenant.AddressId}, @Description = {tenant.Description}, @BankName = {tenant.BankName}, @Director = {tenant.Director}");
        var result = await unitOfWork.Tenants.CreateWithProcedure(tenant);
        logger.LogInformation("Finished calling TenantRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting TenantService, CreateTenantAsync");
        return result;
    }

    public async Task<Response<Guid>> CreateRentAsync(RentToCreateDto rent)
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

    public async Task<Response<EntityEntry<Tenant>>> DeleteTenantAsync(Guid tenantId)
    {
        var result = new Response<EntityEntry<Tenant>>();

        try
        {
            logger.LogInformation("Entering TenantService, DeleteTenantAsync");

            logger.LogInformation("Calling TenantRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.TenantId == tenantId);
            logger.LogInformation("Finished calling TenantRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (!response1.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving tenant by id from database.");
                result.Exceptions = response1.Exceptions;
            }

            logger.LogInformation("Calling TenantRepository, method Delete");
            var response2 = unitOfWork.Tenants.Delete(response1.Body!);
            logger.LogInformation("Finished calling TenantRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (!response2.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while deleting retrieved tenant.");
                result.Exceptions = response2.Exceptions;
            }

            await unitOfWork.SaveAsync();

            result.Body = response2.Body;

            logger.LogInformation("Exiting TenantService, DeleteTenantAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while deleting tenant.", ex));
        }

        return result;
    }

    public async Task<Response<EntityEntry<Tenant>>> UpdateTenantAsync(TenantToGetDto newTenant)
    {
        var result = new Response<EntityEntry<Tenant>>();

        try
        {
            logger.LogInformation("Entering TenantService, UpdateTenantAsync");

            logger.LogInformation("Calling TenantRepository, method GetSingleByConditionAsync");
            var response1 =
                await unitOfWork.Tenants.GetSingleByConditionAsync(tenant => tenant.TenantId == newTenant.TenantId);
            logger.LogInformation("Finished calling TenantRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (!response1.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving tenant by id from database.");
                result.Exceptions = response1.Exceptions;
            }

            response1.Body!.Name = newTenant.Name;
            response1.Body!.Director = newTenant.Director;
            response1.Body!.Description = newTenant.Description;
            response1.Body!.BankName = newTenant.BankName;
            response1.Body!.AddressId = newTenant.AddressId;

            logger.LogInformation("Calling TenantRepository, method Update");
            var response2 = unitOfWork.Tenants.Update(response1.Body!);
            logger.LogInformation("Finished calling TenantRepository, method Update");

            result.TimeStamp = response2.TimeStamp;
            if (!response2.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while updating retrieved tenant.");
                result.Exceptions = response2.Exceptions;
            }

            await unitOfWork.SaveAsync();

            result.Body = response2.Body;

            logger.LogInformation("Exiting TenantService, UpdateTenantAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while updating tenant.", ex));
        }

        return result;
    }

    public async Task<Response<EntityEntry<Model.Library.Rent>>> CancelRentAsync(Guid rentId)
    {
        var result = new Response<EntityEntry<Model.Library.Rent>>();

        try
        {
            logger.LogInformation("Entering TenantService, CancelRentAsync");

            logger.LogInformation("Calling RentRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.Rents.GetSingleByConditionAsync(rent => rent.RentId == rentId);
            logger.LogInformation("Finished calling RentRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;

            if (!response1.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving rent by id from database.");
                result.Exceptions = response1.Exceptions;
            }

            response1.Body!.EndDate = DateTime.Now;

            logger.LogInformation("Calling RentRepository, method Update");
            var response2 = unitOfWork.Rents.Update(response1.Body!);
            logger.LogInformation("Finished calling RentRepository, method Update");

            result.TimeStamp = response2.TimeStamp;
            if (!response2.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while cancelling retrieved rent.");
                result.Exceptions = response2.Exceptions;
            }

            await unitOfWork.SaveAsync();

            result.Body = response2.Body;

            logger.LogInformation("Exiting TenantService, CancelRentAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while canceling rent.", ex));
        }

        return result;
    }

    public async Task<Response<Guid>> CreatePaymentAsync(PaymentToCreateDto payment)
    {
        logger.LogInformation("Entering TenantService, CreatePaymentAsync");

        logger.LogInformation("Calling PaymentRepository, method CreateWithProcedure");
        logger.LogInformation(
            $"Parameters: @TenantId = {payment.TenantId}, @BillId = {payment.BillId}, @Amount = {payment.Amount}");
        var result = await unitOfWork.Payments.CreateWithProcedure(payment);
        logger.LogInformation("Finished calling PaymentRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting TenantService, CreatePaymentAsync");
        return result;
    }
}