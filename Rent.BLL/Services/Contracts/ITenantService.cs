using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Responses;

namespace Rent.BLL.Services.Contracts;

public interface ITenantService
{
    Task<IEnumerable<TenantToGetDto>> GetAllTenantsAsync();

    Task<IEnumerable<BillToGetDto>> GetAllBillsAsync();

    Task<IEnumerable<RentToGetDto>> GetAllRentsAsync();

    Task<TenantToGetDto?> GetTenantByNameAsync(string value);

    Task<TenantToGetDto?> GetTenantByIdAsync(Guid tenantId);

    Task<AddressToGetDto?> GetTenantAddressAsync(Guid tenantId);

    Task<IEnumerable<RentToGetDto>> GetTenantRentsAsync(Guid tenantId);

    Task<IEnumerable<BillToGetDto>> GetTenantBillsAsync(Guid tenantId);

    Task<IEnumerable<BillToGetDto>> GetTenantPaymentsAsync(Guid tenantId);

    Task<CreationResponse> CreateTenantAsync(TenantToCreateDto tenant);

    Task<CreationResponse> CreateRentAsync(RentToCreateDto rent);

    Task<UpdatingResponse> UpdateTenantAsync(TenantToGetDto newTenant);

    Task<UpdatingResponse> DeleteTenantAsync(Guid tenantId);

    Task<UpdatingResponse> CancelRentAsync(Guid rentId);

    Task<CreationResponse> CreatePaymentAsync(PaymentToCreateDto payment);
}