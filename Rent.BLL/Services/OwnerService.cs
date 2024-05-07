using AutoMapper;
using Microsoft.Extensions.Logging;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.UnitOfWork;
using Rent.DAL.RequestsAndResponses;
using Rent.WebAPI.CustomExceptions;

namespace Rent.BLL.Services;

public class OwnerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OwnerService> logger) : IOwnerService
{
    public async Task<GetMultipleResponse<OwnerToGetDto>> GetAllOwnersAsync(params string[] includes)
    {
        var result = new GetMultipleResponse<OwnerToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAllOwnersAsync");

            logger.LogInformation("Calling OwnerRepository, method GetAllAsync");
            var response = await unitOfWork.Owners.GetAllAsync(includes);
            logger.LogInformation("Finished calling OwnerRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                logger.LogError("Exception occured while retrieving all owners from database.");
                throw new ProcessException("Exception occured while retrieving all owners from database.", response.Error);
            }

            logger.LogInformation($"Mapping owners to OwnerToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<OwnerToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetAllOwnersAsync");
        }
        catch (ProcessException ex)
        {
            result.Error = ex;
        }
        catch (AutoMapperMappingException ex)
        {
            result.Error = new Exception("Exception occured while mapping entities.", ex);
        }

        return result;
    }

    public async Task<GetMultipleResponse<OwnerToGetDto>> GetOwnersPartialAsync(GetPartialRequest request, params string[] includes)
    {
        var result = new GetMultipleResponse<OwnerToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetOwnersPartialAsync");

            logger.LogInformation("Calling OwnerRepository, method GetPartialAsync");
            var response = await unitOfWork.Owners.GetPartialAsync(request.Skip, request.Take, includes);
            logger.LogInformation("Finished calling OwnerRepository, method GetPartialAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                logger.LogError("Exception occured while retrieving partial owners from database.");
                throw new ProcessException("Exception occured while retrieving partial owners from database.", response.Error);
            }

            logger.LogInformation($"Mapping owners to OwnerToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<OwnerToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetOwnersPartialAsync");
        }
        catch (ProcessException ex)
        {
            result.Error = ex;
        }
        catch (AutoMapperMappingException ex)
        {
            result.Error = new Exception("Exception occured while mapping entities.", ex);
        }

        return result;
    }

    public async Task<GetMultipleResponse<AssetToGetDto>> GetAllAssetsAsync(params string[] includes)
    {
        var result = new GetMultipleResponse<AssetToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAllAssetsAsync");

            logger.LogInformation("Calling AssetRepository, method GetAllAsync");
            var response = await unitOfWork.Assets.GetAllAsync(includes);
            logger.LogInformation("Finished calling AssetRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                logger.LogError("Exception occured while retrieving all assets from database.");
                throw new ProcessException("Exception occured while retrieving all assets from database.", response.Error);
            }

            logger.LogInformation($"Mapping assets to AssetToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<AssetToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetAllAssetsAsync");
        }
        catch (ProcessException ex)
        {
            result.Error = ex;
        }
        catch (AutoMapperMappingException ex)
        {
            result.Error = new Exception("Exception occured while mapping entities.", ex);
        }

        return result;
    }

    public async Task<GetSingleResponse<OwnerToGetDto>> GetOwnerByIdAsync(Guid ownerId, params string[] includes)
    {
        var result = new GetSingleResponse<OwnerToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetOwnerByIdAsync");

            logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Owners.GetSingleByConditionAsync(owner => owner.OwnerId == ownerId, includes);
            logger.LogInformation("Finished calling OwnerRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                logger.LogError("Exception occured while retrieving owner by id from database.");
                throw new ProcessException("Exception occured while retrieving owner by id from database.", response.Error);
            }

            logger.LogInformation($"Mapping owner to OwnerToGetDto");
            result.Entity = mapper.Map<OwnerToGetDto>(response.Entity);

            logger.LogInformation("Exiting OwnerService, GetOwnerByIdAsync");
        }
        catch (ProcessException ex)
        {
            result.Error = ex;
        }
        catch (AutoMapperMappingException ex)
        {
            result.Error = new Exception("Exception occured while mapping entities.", ex);
        }

        return result;
    }

    public async Task<GetSingleResponse<AssetToGetDto>> GetAssetByIdAsync(Guid assetId, params string[] includes)
    {
        var result = new GetSingleResponse<AssetToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAssetByIdAsync");

            logger.LogInformation("Calling AssetRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Assets.GetSingleByConditionAsync(asset => asset.AssetId == assetId, includes);
            logger.LogInformation("Finished calling AssetRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                logger.LogError("Exception occured while retrieving asset by id from database.");
                throw new ProcessException("Exception occured while retrieving asset by id from database.", response.Error);
            }

            logger.LogInformation($"Mapping asset to AssetToGetDto");
            result.Entity = mapper.Map<AssetToGetDto>(response.Entity);

            logger.LogInformation("Exiting OwnerService, GetAssetByIdAsync");
        }
        catch (ProcessException ex)
        {
            result.Error = ex;
        }
        catch (AutoMapperMappingException ex)
        {
            result.Error = new Exception("Exception occured while mapping entities.", ex);
        }

        return result;
    }

    public async Task<GetMultipleResponse<AssetToGetDto>> GetOwnerAssetsAsync(Guid ownerId, params string[] includes)
    {
        var result = new GetMultipleResponse<AssetToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAllAssetsAsync");

            logger.LogInformation("Calling AssetRepository, method GetByConditionAsync");
            var response = await unitOfWork.Assets.GetByConditionAsync(asset => asset.OwnerId == ownerId, includes);
            logger.LogInformation("Finished calling AssetRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                logger.LogError("Exception occured while retrieving owner's assets id from database.");
                throw new ProcessException("Exception occured while retrieving owner's assets id from database.", response.Error);
            }

            logger.LogInformation($"Mapping assets to AssetToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<AssetToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetAllAssetsAsync");
        }
        catch (ProcessException ex)
        {
            result.Error = ex;
        }
        catch (AutoMapperMappingException ex)
        {
            result.Error = new Exception("Exception occured while mapping entities.", ex);
        }

        return result;
    }

    public async Task<CreationResponse> CreateOwnerAsync(OwnerToCreateDto owner)
    {
        logger.LogInformation("Entering OwnerService, CreateOwnerAsync");

        logger.LogInformation("Calling OwnerRepository, method CreateWithProcedure");
        logger.LogInformation($"Parameters: @Name = {owner.Name}, @AddressId = {owner.AddressId}");
        var result = await unitOfWork.Owners.CreateWithProcedure(owner);

        logger.LogInformation("Finished calling OwnerRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting OwnerService, CreateOwnerAsync");
        return result;
    }

    public async Task<ModifyResponse<Owner>> UpdateOwnerAsync(OwnerToGetDto newOwner)
    {
        var result = new ModifyResponse<Owner>();

        try
        {
            logger.LogInformation("Entering OwnerService, UpdateOwnerAsync");

            logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
            var response1 =
                await unitOfWork.Owners.GetSingleByConditionAsync(owner => owner.OwnerId == newOwner.OwnerId);
            logger.LogInformation("Finished calling OwnerRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (response1.Error is not null)
            {
                logger.LogError("Exception occured while retrieving owner by id from database.");
                throw new ProcessException("Exception occured while retrieving owner by id from database.", response1.Error);
            }

            response1.Entity!.Name = newOwner.Name;
            response1.Entity!.AddressId = newOwner.AddressId;

            logger.LogInformation("Calling OwnerRepository, method Update");
            var response2 = unitOfWork.Owners.Update(response1.Entity!);
            logger.LogInformation("Finished calling OwnerRepository, method Update");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                logger.LogError("Exception occured while updating retrieved owner.");
                throw new ProcessException("Exception occured while updating retrieved owner.", response2.Error);
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting OwnerService, UpdateOwnerAsync");
        }
        catch (ProcessException ex)
        {
            result.Error = ex;
        }
        catch (AutoMapperMappingException ex)
        {
            result.Error = new Exception("Exception occured while mapping entities.", ex);
        }

        return result;
    }

    public async Task<ModifyResponse<Owner>> DeleteOwnerAsync(Guid ownerId)
    {
        var result = new ModifyResponse<Owner>();

        try
        {
            logger.LogInformation("Entering OwnerService, DeleteOwnerAsync");

            logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.Owners.GetSingleByConditionAsync(owner => owner.OwnerId == ownerId);
            logger.LogInformation("Finished calling OwnerRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (response1.Error is not null)
            {
                logger.LogError("Exception occured while retrieving owner by id from database.");
                throw new ProcessException("Exception occured while retrieving owner by id from database.", response1.Error);
            }

            logger.LogInformation("Calling OwnerRepository, method Delete");
            var response2 = unitOfWork.Owners.Delete(response1.Entity!);
            logger.LogInformation("Finished calling OwnerRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                logger.LogError("Exception occured while deleting retrieved owner.");
                throw new ProcessException("Exception occured while deleting retrieved owner.", response2.Error);
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting OwnerService, DeleteOwnerAsync");
        }
        catch (ProcessException ex)
        {
            result.Error = ex;
        }
        catch (AutoMapperMappingException ex)
        {
            result.Error = new Exception("Exception occured while mapping entities.", ex);
        }

        return result;
    }

    public async Task<CreationResponse> CreateAssetAsync(AssetToCreateDto asset)
    {
        logger.LogInformation("Entering OwnerService, CreateAssetAsync");

        logger.LogInformation("Calling AssetRepository, method CreateWithProcedure");
        logger.LogInformation($"Parameters: @OwnerId = {asset.OwnerId}, @RoomId = {asset.RoomId}");
        var result = await unitOfWork.Assets.CreateWithProcedure(asset);
        logger.LogInformation("Finished calling AssetRepository, method CreateWithProcedure");

        logger.LogInformation("Exiting OwnerService, CreateAssetAsync");
        return result;
    }

    public async Task<ModifyResponse<Asset>> DeleteAssetAsync(Guid assetId)
    {
        var result = new ModifyResponse<Asset>();

        try
        {
            logger.LogInformation("Entering OwnerService, DeleteAssetAsync");

            logger.LogInformation("Calling AssetRepository, method GetSingleByConditionAsync");
            var response1 = await unitOfWork.Assets.GetSingleByConditionAsync(asset => asset.AssetId == assetId);
            logger.LogInformation("Finished calling AssetRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response1.TimeStamp;
            if (response1.Error is not null)
            {
                logger.LogError("Exception occured while retrieving asset by id from database.");
                throw new ProcessException("Exception occured while retrieving asset by id from database.", response1.Error);
            }

            logger.LogInformation("Calling AssetRepository, method Delete");
            var response2 = unitOfWork.Assets.Delete(response1.Entity!);
            logger.LogInformation("Finished calling AssetRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                throw new ProcessException("Exception occured while deleting retrieved asset.", response2.Error);
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting OwnerService, DeleteAssetAsync");
        }
        catch (ProcessException ex)
        {
            result.Error = ex;
        }
        catch (AutoMapperMappingException ex)
        {
            result.Error = new Exception("Exception occured while mapping entities.", ex);
        }

        return result;
    }
}