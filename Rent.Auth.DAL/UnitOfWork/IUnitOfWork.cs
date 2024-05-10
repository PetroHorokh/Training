using Rent.Auth.DAL.Repositories.Contract;

namespace Rent.Auth.DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IImageRepository Images { get; }

    Task SaveAsync();
}