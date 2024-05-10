using Rent.Auth.DAL.Context;
using Rent.Auth.DAL.Repositories.Contract;

namespace Rent.Auth.DAL.UnitOfWork;

public class UnitOfWork(
    AuthRentContext context,
    Lazy<IImageRepository> imageRepository) : IUnitOfWork
{
    private readonly Lazy<IImageRepository> _imageRepository = imageRepository;

    public IImageRepository Images => _imageRepository.Value;
    
    public async Task SaveAsync() => await context.SaveChangesAsync();
    public void Dispose() => context.Dispose();
}