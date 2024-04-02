﻿using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RepositoryBase;
using Rent.DAL.Responses;

namespace Rent.DAL.Repositories.Contracts;

public interface IAccommodationRepository : IRepositoryBase<Accommodation>
{
    Task<CreationDictionaryResponse> CreateWithProcedure(AccommodationToCreateDto accommodation);
}