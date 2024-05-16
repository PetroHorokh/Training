using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.UnitOfWork;
using Rent.DAL.RequestsAndResponses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Rent.ExceptionLibrary;
using Rent.Response.Library;

namespace Rent.BLL.Services;

/// <summary>
/// Service for working with owners
/// </summary>
/// <param name="unitOfWork">Parameter to use UnitOfWork pattern implemented in Dal layer</param>
/// <param name="mapper">Parameter to use mapper with provided profiles</param>
/// <param name="logger">Parameter to use logging</param>
/// <param name="memoryCache">Parameter to use in-memory caching</param>
public class OwnerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OwnerService> logger, IMemoryCache memoryCache) : IOwnerService
{
    /// <summary>
    /// Property for setting root cache key for signalizing in-memory cache is used by current service
    /// </summary>
    private const string RootCacheKey = "Owners";
    /// <summary>
    /// Property for setting SlidingExpiration after which caching of specific entity is being prolonged
    /// </summary>
    private const int SlidingExpiration = 2;
    /// <summary>
    /// Property for setting AbsoluteExpiration which is upper limit for caching specific entity
    /// </summary>
    private const int AbsoluteExpiration = 10;

    /// <summary>
    /// Get all owners from UnitOfWork Owners with method GetAllAsync
    /// </summary>
    /// <param name="includes">Parameter to use include with EF to add necessary related tables</param>
    /// <exception cref="ProcessException">Exception thrown when error occured while making a request to database</exception>
    /// <exception cref="AutoMapperMappingException">Exception thrown when error occured while mapping entities</exception>
    /// <returns>Returns <see cref="GetMultipleResponse{OwnerToGetDto}"/> entity with either IEnumerable of <see cref="OwnerToGetDto"/> entities or thrown exception</returns>
    public async Task<Response<IEnumerable<OwnerToGetDto>>> GetAllOwnersAsync(params string[] includes)
    {
        var result = new Response<IEnumerable<OwnerToGetDto>>();
        var cacheKey = RootCacheKey + "All";

        try
        {
            logger.LogInformation("Entering OwnerService, GetAllOwnersAsync");

            logger.LogInformation("Checking memory cache for owners");
            if (!memoryCache.TryGetValue(cacheKey, out List<OwnerToGetDto>? owners))
            {
                logger.LogInformation("Calling OwnerRepository, method GetAllAsync");
                var response = await unitOfWork.Owners.GetAllAsync(includes);
                logger.LogInformation("Finished calling OwnerRepository, method GetAllAsync");

                result.TimeStamp = response.TimeStamp;
                if (!response.Exceptions.IsNullOrEmpty())
                {
                    logger.LogError("Exception occured while retrieving all owners from database.");
                    result.Exceptions = response.Exceptions;
                }

                if(response.Body.IsNullOrEmpty())
                    owners = new List<OwnerToGetDto>();
                else
                {
                    logger.LogInformation($"Mapping owners to OwnerToGetDto");
                    owners = response.Body!.Select(mapper.Map<OwnerToGetDto>).ToList();
                }

                logger.LogInformation($"Adding tenants into memory cache with SlidingExpiration - {SlidingExpiration}; AbsoluteExpiration - {AbsoluteExpiration}");
                memoryCache.Set(cacheKey, owners,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(SlidingExpiration))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(AbsoluteExpiration)));

                result.Body = owners;
            }
            else
            {
                result.Body = owners;
            }

            logger.LogInformation("Exiting OwnerService, GetAllOwnersAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving all owners from database.", ex));
        }

        return result;
    }

    public async Task<Response<IEnumerable<OwnerToGetDto>>> GetOwnersPartialAsync(GetPartialRequest request, params string[] includes)
    {
        var result = new Response<IEnumerable<OwnerToGetDto>>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetOwnersPartialAsync");

            logger.LogInformation("Calling OwnerRepository, method GetPartialAsync");
            var response = await unitOfWork.Owners.GetPartialAsync(request.Skip, request.Take, includes);
            logger.LogInformation("Finished calling OwnerRepository, method GetPartialAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving partial owners from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping owners to OwnerToGetDto");
            result.Body = response.Body!.Select(mapper.Map<OwnerToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetOwnersPartialAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving partial owners from database.", ex));
        }

        return result;
    }

    public async Task<Response<IEnumerable<AssetToGetDto>>> GetAllAssetsAsync(params string[] includes)
    {
        var result = new Response<IEnumerable<AssetToGetDto>>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAllAssetsAsync");

            logger.LogInformation("Calling AssetRepository, method GetAllAsync");
            var response = await unitOfWork.Assets.GetAllAsync(includes);
            logger.LogInformation("Finished calling AssetRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving all assets from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping assets to AssetToGetDto");
            result.Body = response.Body!.Select(mapper.Map<AssetToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetAllAssetsAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving all assets from database.", ex));
        }

        return result;
    }

    public async Task<Response<OwnerToGetDto>> GetOwnerByIdAsync(Guid ownerId, params string[] includes)
    {
        var result = new Response<OwnerToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetOwnerByIdAsync");

            logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Owners.GetSingleByConditionAsync(owner => owner.OwnerId == ownerId, includes);
            logger.LogInformation("Finished calling OwnerRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving owner by id from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping owner to OwnerToGetDto");
            result.Body = mapper.Map<OwnerToGetDto>(response.Body);

            logger.LogInformation("Exiting OwnerService, GetOwnerByIdAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving owner by id from database.", ex));
        }

        return result;
    }

    public async Task<Response<AssetToGetDto>> GetAssetByIdAsync(Guid assetId, params string[] includes)
    {
        var result = new Response<AssetToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAssetByIdAsync");

            logger.LogInformation("Calling AssetRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Assets.GetSingleByConditionAsync(asset => asset.AssetId == assetId, includes);
            logger.LogInformation("Finished calling AssetRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving asset by id from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping asset to AssetToGetDto");
            result.Body = mapper.Map<AssetToGetDto>(response.Body);

            logger.LogInformation("Exiting OwnerService, GetAssetByIdAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving asset by id from database.", ex));
        }

        return result;
    }

    public async Task<Response<IEnumerable<AssetToGetDto>>> GetOwnerAssetsAsync(Guid ownerId, params string[] includes)
    {
        var result = new Response<IEnumerable<AssetToGetDto>>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAllAssetsAsync");

            logger.LogInformation("Calling AssetRepository, method GetByConditionAsync");
            var response = await unitOfWork.Assets.GetByConditionAsync(asset => asset.OwnerId == ownerId, includes);
            logger.LogInformation("Finished calling AssetRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (!response.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving owner's assets id from database.");
                result.Exceptions = response.Exceptions;
            }

            logger.LogInformation($"Mapping assets to AssetToGetDto");
            result.Body = response.Body!.Select(mapper.Map<AssetToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetAllAssetsAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while retrieving owner's assets id from database.", ex));
        }

        return result;
    }

    public async Task<Response<Guid>> CreateOwnerAsync(OwnerToCreateDto owner)
    {
        logger.LogInformation("Entering OwnerService, CreateOwnerAsync");

        logger.LogInformation("Calling OwnerRepository, method CreateWithProcedure");
        logger.LogInformation($"Parameters: @Name = {owner.Name}, @AddressId = {owner.AddressId}");
        var result = await unitOfWork.Owners.CreateWithProcedure(owner);

        logger.LogInformation("Finished calling OwnerRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting OwnerService, CreateOwnerAsync");
        return result;
    }

    public async Task<Response<EntityEntry<Owner>>> UpdateOwnerAsync(OwnerToGetDto newOwner)
    {
        var result = new Response<EntityEntry<Owner>>();

        try
        {
            logger.LogInformation("Entering OwnerService, UpdateOwnerAsync");

            logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
            var response1 =
                await unitOfWork.Owners.GetSingleByConditionAsync(owner => owner.OwnerId == newOwner.OwnerId);
            logger.LogInformation("Finished calling OwnerRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (!response1.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving owner by id from database.");
                result.Exceptions = response1.Exceptions;
            }

            response1.Body!.Name = newOwner.Name;
            response1.Body!.AddressId = newOwner.AddressId;

            logger.LogInformation("Calling OwnerRepository, method Update");
            var response2 = unitOfWork.Owners.Update(response1.Body!);
            logger.LogInformation("Finished calling OwnerRepository, method Update");

            result.TimeStamp = response2.TimeStamp;
            if (!response2.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while updating retrieved owner.");
                result.Exceptions = response2.Exceptions;
            }

            await unitOfWork.SaveAsync();

            result.Body = response2.Body;

            logger.LogInformation("Exiting OwnerService, UpdateOwnerAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while updating owner.", ex));
        }

        return result;
    }

    public async Task<Response<EntityEntry<Owner>>> DeleteOwnerAsync(Guid ownerId)
    {
        var result = new Response<EntityEntry<Owner>>();

        try
        {
            logger.LogInformation("Entering OwnerService, DeleteOwnerAsync");

            logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.Owners.GetSingleByConditionAsync(owner => owner.OwnerId == ownerId);
            logger.LogInformation("Finished calling OwnerRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (!response1.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving owner by id from database.");
                result.Exceptions = response1.Exceptions;
            }

            logger.LogInformation("Calling OwnerRepository, method Delete");
            var response2 = unitOfWork.Owners.Delete(response1.Body!);
            logger.LogInformation("Finished calling OwnerRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (!response2.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while deleting retrieved owner.");
                result.Exceptions = response2.Exceptions;
            }

            await unitOfWork.SaveAsync();

            result.Body = response2.Body;

            logger.LogInformation("Exiting OwnerService, DeleteOwnerAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while deleting owner.", ex));
        }
        
        return result;
    }

    public async Task<Response<Guid>> CreateAssetAsync(AssetToCreateDto asset)
    {
        logger.LogInformation("Entering OwnerService, CreateAssetAsync");

        logger.LogInformation("Calling AssetRepository, method CreateWithProcedure");
        logger.LogInformation($"Parameters: @OwnerId = {asset.OwnerId}, @RoomId = {asset.RoomId}");
        var result = await unitOfWork.Assets.CreateWithProcedure(asset);
        logger.LogInformation("Finished calling AssetRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting OwnerService, CreateAssetAsync");
        return result;
    }

    public async Task<Response<EntityEntry<Asset>>> DeleteAssetAsync(Guid assetId)
    {
        var result = new Response<EntityEntry<Asset>>();

        try
        {
            logger.LogInformation("Entering OwnerService, DeleteAssetAsync");

            logger.LogInformation("Calling AssetRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.Assets.GetSingleByConditionAsync(asset => asset.AssetId == assetId);
            logger.LogInformation("Finished calling AssetRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (!response1.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while retrieving asset by id from database.");
                result.Exceptions = response1.Exceptions;
            }

            logger.LogInformation("Calling AssetRepository, method Delete");
            var response2 = unitOfWork.Assets.Delete(response1.Body!);
            logger.LogInformation("Finished calling AssetRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (!response2.Exceptions.IsNullOrEmpty())
            {
                logger.LogError("Exception occured while deleting retrieved asset.");
                result.Exceptions = response2.Exceptions;
            }

            await unitOfWork.SaveAsync();

            result.Body = response2.Body;

            logger.LogInformation("Exiting OwnerService, DeleteAssetAsync");
        }
        catch (AutoMapperMappingException ex)
        {
            result.Exceptions.Add(new AutoMapperMappingException("Exception occured while mapping entities.", ex));
        }
        catch (Exception ex)
        {
            result.Exceptions.Add(new ProcessException("Exception occured while deleting asset.", ex));
        }

        return result;
    }
}