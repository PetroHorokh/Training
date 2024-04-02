using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;

namespace Rent.DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IAccommodationRepository Accommodations { get; }
    IAccommodationRoomRepository AccommodationRooms { get; }
    IAddressRepository Addresses { get; }
    IAssetRepository Assets { get; }
    IBillRepository Bills { get; }
    IImpostRepository Imposts { get; }
    IOwnerRepository Owners { get; }
    IPaymentRepository Payments { get; }
    IPriceRepository Prices { get; }
    IRentRepository Rents { get; }
    IRoomRepository Rooms { get; }
    IRoomTypeRepository RoomTypes { get; }
    ITenantRepository Tenants { get; }
    IViewRepository Views { get; }

    Task SaveAsync();
}