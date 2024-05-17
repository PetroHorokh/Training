using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RequestsAndResponses;
using Rent.Response.Library;

namespace Rent.BLL.Services.Contracts;

public interface IOwnerService
{
    Task<Response<IEnumerable<OwnerToGetDto>>> GetAllOwnersAsync(params string[] includes);

    Task<Response<IEnumerable<OwnerToGetDto>>> GetOwnersPartialAsync(GetPartialRequest request, params string[] includes);

    Task<Response<IEnumerable<AssetToGetDto>>> GetAllAssetsAsync(params string[] includes);

    Task<Response<OwnerToGetDto>> GetOwnerByIdAsync(Guid ownerId, params string[] includes);

    Task<Response<AssetToGetDto>> GetAssetByIdAsync(Guid assetId, params string[] includes);

    Task<Response<IEnumerable<AssetToGetDto>>> GetOwnerAssetsAsync(Guid ownerId, params string[] includes);

    Task<Response<Guid>> CreateOwnerAsync(OwnerToCreateDto owner);

    Task<Response<EntityEntry<Owner>>> UpdateOwnerAsync(OwnerToGetDto newOwner);

    Task<Response<EntityEntry<Owner>>> DeleteOwnerAsync(Guid ownerId);

    Task<Response<Guid>> CreateAssetAsync(AssetToCreateDto asset);

    Task<Response<EntityEntry<Asset>>> DeleteAssetAsync(Guid assetId);
}