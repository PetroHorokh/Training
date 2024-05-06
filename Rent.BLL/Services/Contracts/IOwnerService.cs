using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RequestsAndResponses;

namespace Rent.BLL.Services.Contracts;

public interface IOwnerService
{
    Task<GetMultipleResponse<OwnerToGetDto>> GetAllOwnersAsync(params string[] includes);

    Task<GetMultipleResponse<OwnerToGetDto>> GetOwnersPartialAsync(GetPartialRequest request, params string[] includes);

    Task<GetMultipleResponse<AssetToGetDto>> GetAllAssetsAsync(params string[] includes);

    Task<GetSingleResponse<OwnerToGetDto>> GetOwnerByIdAsync(Guid ownerId, params string[] includes);

    Task<GetSingleResponse<AssetToGetDto>> GetAssetByIdAsync(Guid assetId, params string[] includes);

    Task<GetMultipleResponse<AssetToGetDto>> GetOwnerAssetsAsync(Guid ownerId, params string[] includes);

    Task<CreationResponse> CreateOwnerAsync(OwnerToCreateDto owner);

    Task<ModifyResponse<Owner>> UpdateOwnerAsync(OwnerToGetDto newOwner);

    Task<ModifyResponse<Owner>> DeleteOwnerAsync(Guid ownerId);

    Task<CreationResponse> CreateAssetAsync(AssetToCreateDto asset);

    Task<ModifyResponse<Asset>> DeleteAssetAsync(Guid assetId);
}