using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RepositoryBase;
using Rent.DAL.RequestsAndResponses;
using Rent.Response.Library;

namespace Rent.DAL.Repositories.Contracts;

public interface IAssetRepository : IRepositoryBase<Asset>
{
    Task<Response<Guid>> CreateWithProcedure(AssetToCreateDto asset);
}