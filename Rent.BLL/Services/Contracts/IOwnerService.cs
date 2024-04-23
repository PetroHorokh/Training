using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RequestsAndResponses;

namespace Rent.BLL.Services.Contracts;

public interface IOwnerService
{
    Task<GetMultipleResponse<OwnerToGetDto>> GetAllOwnersAsync();

    Task<GetMultipleResponse<OwnerToGetDto>> GetOwnersPartialAsync(GetPartialRequest request);

    Task<GetMultipleResponse<AssetToGetDto>> GetAllAssetsAsync();

    Task<GetSingleResponse<OwnerToGetDto>> GetOwnerByIdAsync(Guid ownerId);

    Task<GetSingleResponse<AssetToGetDto>> GetAssetByIdAsync(Guid assetId);

    Task<GetMultipleResponse<AssetToGetDto>> GetOwnerAssetsAsync(Guid ownerId);

    Task<CreationResponse> CreateOwnerAsync(OwnerToCreateDto owner);

    Task<ModifyResponse<Owner>> UpdateOwnerAsync(OwnerToGetDto newOwner);

    Task<ModifyResponse<Owner>> DeleteOwnerAsync(Guid ownerId);

    Task<CreationResponse> CreateAssetAsync(AssetToCreateDto asset);

    Task<ModifyResponse<Asset>> DeleteAssetAsync(Guid assetId);
}