using Rent.DAL.Context;
using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;

namespace Rent.DAL.UnitOfWork;

public class UnitOfWork(
    RentContext rentContext,
    Lazy<IAccommodationRepository> accommodationRepository,
    Lazy<IAccommodationRoomRepository> accommodationRoomRepository,
    Lazy<IAddressRepository> addressRepository,
    Lazy<IAssetRepository> assetRepository,
    Lazy<IBillRepository> billRepository,
    Lazy<IImpostRepository> impostRepository,
    Lazy<IOwnerRepository> ownerRepository,
    Lazy<IPaymentRepository> paymentRepository,
    Lazy<IPriceRepository> priceRepository,
    Lazy<IRentRepository> rentRepository,
    Lazy<IRoomRepository> roomRepository,
    Lazy<IRoomTypeRepository> roomTypeRepository,
    Lazy<ITenantRepository> tenantRepository,
    Lazy<IUserRepository> userRepository,
    Lazy<IRepositoryBase<Role>> roleRepository,
    Lazy<IRepositoryBase<UserRole>> userRoleRepository,
    Lazy<IViewRepository> viewRepository) : IUnitOfWork
{
    private readonly RentContext _rentContext = rentContext;

    private readonly Lazy<IAccommodationRepository> _accommodationRepository = accommodationRepository;
    private readonly Lazy<IAccommodationRoomRepository> _accommodationRoomRepository = accommodationRoomRepository;
    private readonly Lazy<IAddressRepository> _addressRepository = addressRepository;
    private readonly Lazy<IAssetRepository> _assetRepository = assetRepository;
    private readonly Lazy<IBillRepository> _billRepository = billRepository;
    private readonly Lazy<IImpostRepository> _impostRepository = impostRepository;
    private readonly Lazy<IOwnerRepository> _ownerRepository = ownerRepository;
    private readonly Lazy<IPaymentRepository> _paymentRepository = paymentRepository;
    private readonly Lazy<IPriceRepository> _priceRepository = priceRepository;
    private readonly Lazy<IRentRepository> _rentRepository = rentRepository;
    private readonly Lazy<IRoomRepository> _roomRepository = roomRepository;
    private readonly Lazy<IRoomTypeRepository> _roomTypeRepository = roomTypeRepository;
    private readonly Lazy<ITenantRepository> _tenantRepository = tenantRepository;
    private readonly Lazy<IUserRepository> _userRepository = userRepository;
    private readonly Lazy<IRepositoryBase<Role>> _roleRepository = roleRepository;
    private readonly Lazy<IRepositoryBase<UserRole>> _userRoleRepository = userRoleRepository;
    private readonly Lazy<IViewRepository> _viewRepository = viewRepository;

    public IAccommodationRepository Accommodations => _accommodationRepository.Value;
    public IAccommodationRoomRepository AccommodationRooms => _accommodationRoomRepository.Value;
    public IAddressRepository Addresses => _addressRepository.Value;
    public IAssetRepository Assets => _assetRepository.Value;
    public IBillRepository Bills => _billRepository.Value;
    public IImpostRepository Imposts => _impostRepository.Value;
    public IOwnerRepository Owners => _ownerRepository.Value;
    public IPaymentRepository Payments => _paymentRepository.Value;
    public IPriceRepository Prices => _priceRepository.Value;
    public IRentRepository Rents => _rentRepository.Value;
    public IRoomRepository Rooms => _roomRepository.Value;
    public IRoomTypeRepository RoomTypes => _roomTypeRepository.Value;
    public ITenantRepository Tenants => _tenantRepository.Value;
    public IUserRepository Users => _userRepository.Value;
    public IRepositoryBase<Role> Roles => _roleRepository.Value;
    public IRepositoryBase<UserRole> UserRoles => _userRoleRepository.Value;
    public IViewRepository Views => _viewRepository.Value;

    public async Task SaveAsync() => await _rentContext.SaveChangesAsync();
    public void Dispose() => _rentContext.Dispose();
}