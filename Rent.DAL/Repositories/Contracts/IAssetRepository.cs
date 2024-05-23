using Rent.DAL.RepositoryBase;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories.Contracts;

public interface IAssetRepository : IRepositoryBase<Asset>
{
    Task<Response<Guid>> CreateWithProcedure(AssetToCreateDto asset);
}