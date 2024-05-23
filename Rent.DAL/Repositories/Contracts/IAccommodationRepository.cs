using Rent.DAL.RepositoryBase;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories.Contracts;

public interface IAccommodationRepository : IRepositoryBase<Accommodation>
{
    Task<Response<int>> CreateWithProcedure(AccommodationToCreateDto accommodation);
}