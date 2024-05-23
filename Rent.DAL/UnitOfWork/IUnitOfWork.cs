using Rent.DAL.Repositories.Contracts;

namespace Rent.DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IAccommodationRepository Accommodations { get; }
    IAccommodationRoomRepository AccommodationRooms { get; }
    IAssetRepository Assets { get; }
    IBillRepository Bills { get; }
    IOwnerRepository Owners { get; }
    IPaymentRepository Payments { get; }
    IRentRepository Rents { get; }
    IRoomRepository Rooms { get; }
    IRoomTypeRepository RoomTypes { get; }
    ITenantRepository Tenants { get; }
    IViewRepository Views { get; }

    Task SaveAsync();
}