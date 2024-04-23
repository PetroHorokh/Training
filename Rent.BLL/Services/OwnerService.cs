using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.UnitOfWork;
using System.Data.SqlTypes;
using Rent.DAL.RequestsAndResponses;
using Azure.Core;

namespace Rent.BLL.Services;

public class OwnerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OwnerService> logger) : IOwnerService
{
    public async Task<GetMultipleResponse<OwnerToGetDto>> GetAllOwnersAsync()
    {
        var result = new GetMultipleResponse<OwnerToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAllOwnersAsync");

            logger.LogInformation("Calling OwnerRepository, method GetAllAsync");
            var response = await unitOfWork.Owners.GetAllAsync();
            logger.LogInformation("Finished calling OwnerRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping owners to OwnerToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<OwnerToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetAllOwnersAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<OwnerToGetDto>> GetOwnersPartialAsync(GetPartialRequest request)
    {
        var result = new GetMultipleResponse<OwnerToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetOwnersPartialAsync");

            logger.LogInformation("Calling OwnerRepository, method GetPartialAsync");
            var response = await unitOfWork.Owners.GetPartialAsync(request.Skip, request.Take);
            logger.LogInformation("Finished calling OwnerRepository, method GetPartialAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping owners to OwnerToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<OwnerToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetOwnersPartialAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<AssetToGetDto>> GetAllAssetsAsync()
    {
        var result = new GetMultipleResponse<AssetToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAllAssetsAsync");

            logger.LogInformation("Calling AssetRepository, method GetAllAsync");
            var response = await unitOfWork.Assets.GetAllAsync();
            logger.LogInformation("Finished calling AssetRepository, method GetAllAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping assets to AssetToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<AssetToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetAllAssetsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetSingleResponse<OwnerToGetDto>> GetOwnerByIdAsync(Guid ownerId)
    {
        var result = new GetSingleResponse<OwnerToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetOwnerByIdAsync");

            logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Owners.GetSingleByConditionAsync(owner => owner.OwnerId == ownerId);
            logger.LogInformation("Finished calling OwnerRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping owner to OwnerToGetDto");
            result.Entity = mapper.Map<OwnerToGetDto>(response.Entity);

            logger.LogInformation("Exiting OwnerService, GetOwnerByIdAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetSingleResponse<AssetToGetDto>> GetAssetByIdAsync(Guid assetId)
    {
        var result = new GetSingleResponse<AssetToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAssetByIdAsync");

            logger.LogInformation("Calling AssetRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Assets.GetSingleByConditionAsync(asset => asset.AssetId == assetId);
            logger.LogInformation("Finished calling AssetRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping asset to AssetToGetDto");
            result.Entity = mapper.Map<AssetToGetDto>(response.Entity);

            logger.LogInformation("Exiting OwnerService, GetAssetByIdAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }

    public async Task<GetMultipleResponse<AssetToGetDto>> GetOwnerAssetsAsync(Guid ownerId)
    {
        var result = new GetMultipleResponse<AssetToGetDto>();

        try
        {
            logger.LogInformation("Entering OwnerService, GetAllAssetsAsync");

            logger.LogInformation("Calling AssetRepository, method GetByConditionAsync");
            var response = await unitOfWork.Assets.GetByConditionAsync(asset => asset.OwnerId == ownerId);
            logger.LogInformation("Finished calling AssetRepository, method GetByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation($"Mapping assets to AssetToGetDto");
            result.Collection = response.Collection!.Select(mapper.Map<AssetToGetDto>);

            logger.LogInformation("Exiting OwnerService, GetAllAssetsAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
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
                throw response1.Error;
            }

            response1.Entity!.Name = newOwner.Name;
            response1.Entity!.AddressId = newOwner.AddressId;

            logger.LogInformation("Calling OwnerRepository, method Update");
            var response2 = unitOfWork.Owners.Update(response1.Entity!);
            logger.LogInformation("Finished calling OwnerRepository, method Update");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                throw response2.Error;
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting OwnerService, UpdateOwnerAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
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
                throw response1.Error;
            }

            logger.LogInformation("Calling OwnerRepository, method Delete");
            var response2 = unitOfWork.Owners.Delete(response1.Entity!);
            logger.LogInformation("Finished calling OwnerRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                throw response2.Error;
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting OwnerService, DeleteOwnerAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
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
                throw response1.Error;
            }

            logger.LogInformation("Calling AssetRepository, method Delete");
            var response2 = unitOfWork.Assets.Delete(response1.Entity!);
            logger.LogInformation("Finished calling AssetRepository, method Delete");

            result.TimeStamp = response2.TimeStamp;
            if (response2.Error is not null)
            {
                throw response2.Error;
            }

            await unitOfWork.SaveAsync();

            result.Status = response2.Status;

            logger.LogInformation("Exiting OwnerService, DeleteAssetAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
    }
}