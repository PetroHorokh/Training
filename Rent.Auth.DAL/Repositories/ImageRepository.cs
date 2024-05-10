using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rent.Auth.DAL.Context;
using Rent.Auth.DAL.Models;
using Rent.Auth.DAL.Repositories.Contract;
using Rent.Auth.DAL.RepositoryBase;

namespace Rent.Auth.DAL.Repositories;

public class ImageRepository(AuthRentContext context)
    : RepositoryBase<Image>(context), IImageRepository
{
    
}