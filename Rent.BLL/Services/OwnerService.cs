using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.UnitOfWork;
using System.Data.SqlTypes;
using Rent.DAL.Responses;

namespace Rent.BLL.Services;

public class OwnerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OwnerService> logger) : IOwnerService
{
    public async Task<IEnumerable<OwnerToGetDto>> GetAllOwnersAsync()
    {
        logger.LogInformation("Entering OwnerService, GetAllOwnersAsync");

        logger.LogInformation("Calling OwnerRepository, method GetAllAsync");
        var owners = (await unitOfWork.Owners.GetByConditionAsync(_ => true, owner => owner.Address!)).ToList();
        logger.LogInformation("Finished calling OwnerRepository, method GetAllAsync");

        logger.LogInformation($"Mapping owners to OwnerToGetDto");
        var result = owners.Select(mapper.Map<OwnerToGetDto>);

        logger.LogInformation("Exiting OwnerService, GetAllOwnersAsync");
        return result.ToList();
    }

    public async Task<IEnumerable<AssetToGetDto>> GetAllAssetsAsync()
    {
        logger.LogInformation("Entering OwnerService, GetAllAssetsAsync");

        logger.LogInformation("Calling AssetRepository, method GetAllAsync");
        var assets = await unitOfWork.Assets.GetAllAsync();
        logger.LogInformation("Finished calling AssetRepository, method GetAllAsync");

        logger.LogInformation($"Mapping owners to OwnerToGetDto");
        var result = assets.Select(mapper.Map<AssetToGetDto>);

        logger.LogInformation("Exiting OwnerService, GetAllAssetsAsync");
        return result.ToList();
    }

    public async Task<OwnerToGetDto?> GetOwnerByIdAsync(Guid ownerId)
    {
        logger.LogInformation("Entering OwnerService, GetOwnerByIdAsync");

        logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameter: ownerId = {ownerId}");
        var owner = await unitOfWork.Owners.GetSingleByConditionAsync(owner => owner.OwnerId == ownerId, owner => owner.Address!);
        logger.LogInformation("Finished calling OwnerRepository, method GetSingleByConditionAsync");

        logger.LogInformation($"Mapping owner to OwnerToGetDto");
        var result = mapper.Map<OwnerToGetDto>(owner);

        logger.LogInformation("Exiting OwnerService, GetOwnerByIdAsync");
        return result;
    }

    public async Task<AssetToGetDto?> GetAssetByIdAsync(Guid assetId)
    {
        logger.LogInformation("Entering OwnerService, GetAssetByIdAsync");

        logger.LogInformation("Calling AssetRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameter: assetId = {assetId}");
        var asset = await unitOfWork.Assets.GetSingleByConditionAsync(asset => asset.AssetId == assetId);
        logger.LogInformation("Finished calling AssetRepository, method GetSingleByConditionAsync");

        logger.LogInformation($"Mapping asset to AssetToGetDto");
        var result = mapper.Map<AssetToGetDto>(asset);

        logger.LogInformation("Exiting OwnerService, GetAssetByIdAsync");
        return result;
    }

    public async Task<AddressToGetDto?> GetOwnerAddressAsync(Guid ownerId)
    {
        logger.LogInformation("Entering OwnerService, GetOwnerAddressAsync");

        logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameter: OwnerId = {ownerId}");
        var address = (await unitOfWork.Owners.GetSingleByConditionAsync(owner => owner.OwnerId == ownerId,
            owner => owner.Address!))!.Address;
        logger.LogInformation("Finished calling OwnerRepository, method GetSingleByConditionAsync");

        logger.LogInformation($"Mapping tenant to TenantToGetDto");
        var result = mapper.Map<AddressToGetDto>(address);

        logger.LogInformation("Exiting OwnerService, GetOwnerAddressAsync");
        return result;
    }

    public async Task<IEnumerable<AssetToGetDto>> GetOwnerAssetsAsync(Guid ownerId)
    {
        logger.LogInformation("Entering OwnerService, GetOwnerAssetsAsync");

        logger.LogInformation("Calling AssetRepository, method GetByConditionAsync");
        var assets = await unitOfWork.Assets.GetByConditionAsync(asset => asset.OwnerId == ownerId);
        logger.LogInformation("Finished calling AssetRepository, method GetByConditionAsync");

        logger.LogInformation($"Mapping owner to OwnerToGetDto");
        var result = assets.Select(mapper.Map<AssetToGetDto>);

        logger.LogInformation("Exiting OwnerService, GetOwnerAssetsAsync");
        return result.ToList();
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

    public async Task<UpdatingResponse> UpdateOwnerAsync(OwnerToGetDto newOwner)
    {
        logger.LogInformation("Entering OwnerService, UpdateOwnerAsync");

        Exception? error = null;

        logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameters: AddressId = {newOwner.AddressId}, Name = {newOwner.Name}");
        var owner = await unitOfWork.Owners.GetSingleByConditionAsync(owner => owner.OwnerId == newOwner.OwnerId);
        logger.LogInformation("Finished calling OwnerRepository, method GetSingleByConditionAsync");

        try
        {
            if (owner != null)
            {
                owner.Name = newOwner.Name;
                owner.AddressId = newOwner.AddressId;

                logger.LogInformation("Calling OwnerRepository, method Update");
                unitOfWork.Owners.Update(owner);
                logger.LogInformation("Finished calling OwnerRepository, method Update");

                await unitOfWork.SaveAsync();
            }
            else
            {
                throw new SqlNullValueException("Couldn't find owner");
            }
        }
        catch (DbUpdateException ex)
        {
            logger.LogInformation($"An error occured while deleting Owner entity: {ex.InnerException}");
            error = ex;
        }
        catch (SqlNullValueException ex)
        {
            logger.LogInformation($"An error occured while deleting Owner entity: {ex.InnerException}");
            error = ex;
        }

        logger.LogInformation("Exiting OwnerService, UpdateOwnerAsync");
        return new UpdatingResponse() { DateTime = DateTime.Now, Error = error };
    }

    public async Task<UpdatingResponse> DeleteOwnerAsync(Guid ownerId)
    {
        logger.LogInformation("Entering OwnerService, DeleteOwnerAsync");

        Exception? error = null;

        logger.LogInformation("Calling OwnerRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameter: OwnerId = {ownerId}");
        var owner = await unitOfWork.Owners.GetSingleByConditionAsync(room => room.OwnerId == ownerId);
        logger.LogInformation("Finished calling RoomRepository, method GetSingleByConditionAsync");
        try
        {
            if (owner != null)
            {
                logger.LogInformation("Calling OwnerRepository, method Delete");
                unitOfWork.Owners.Delete(owner);
                logger.LogInformation("Finished calling OwnerRepository, method Delete");

                await unitOfWork.SaveAsync();
            }
            else
            {
                throw new SqlNullValueException("Couldn't find owner");
            }
        }
        catch (DbUpdateException ex)
        {
            logger.LogInformation($"An error occured while deleting Owner entity: {ex.InnerException}");
            error = ex;
        }
        catch (SqlNullValueException ex)
        {
            logger.LogInformation($"An error occured while deleting Owner entity: {ex.InnerException}");
            error = ex;
        }

        logger.LogInformation("Exiting OwnerService, DeleteOwnerAsync");
        return new UpdatingResponse() { DateTime = DateTime.Now, Error = error };
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

    public async Task<UpdatingResponse> DeleteAssetAsync(Guid assetId)
    {
        logger.LogInformation("Entering OwnerService, DeleteAssetAsync");

        Exception? error = null;

        logger.LogInformation("Calling AssetRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameter: AssetId = {assetId}");
        var asset = await unitOfWork.Assets.GetSingleByConditionAsync(asset => asset.AssetId == assetId);
        logger.LogInformation("Finished calling AssetRepository, method GetSingleByConditionAsync");
        try
        {
            if (asset != null)
            {
                logger.LogInformation("Calling AssetRepository, method Delete");
                unitOfWork.Assets.Delete(asset);
                logger.LogInformation("Finished calling AssetRepository, method Delete");

                await unitOfWork.SaveAsync();
            }
            else
            {
                throw new SqlNullValueException("Couldn't find asset");
            }
        }
        catch (DbUpdateException ex)
        {
            logger.LogInformation($"An error occured while deleting Asset entity: {ex.InnerException}");
            error = ex;
        }
        catch (SqlNullValueException ex)
        {
            logger.LogInformation($"An error occured while deleting Asset entity: {ex.InnerException}");
            error = ex;
        }

        logger.LogInformation("Exiting OwnerService, DeleteAssetAsync");
        return new UpdatingResponse() { DateTime = DateTime.Now, Error = error };
    }
}