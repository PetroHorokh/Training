using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.BLL.Services.Contracts;

public interface ITenantService
{
    Task<Response<IEnumerable<TenantToGetDto>>> GetAllTenantsAsync(params string[] includes);

    Task<Response<IEnumerable<TenantToGetDto>>> GetTenantsPartialAsync(GetPartialRequest request, params string[] includes);

    Task<Response<IEnumerable<TenantToGetDto>>> GetFilterTenantsAsync(GetFilteredRequest filter, params string[] includes);

    Task<Response<IEnumerable<BillToGetDto>>> GetAllBillsAsync(params string[] includes);

    Task<Response<IEnumerable<RentToGetDto>>> GetAllRentsAsync(params string[] includes);

    Task<Response<TenantToGetDto>> GetTenantByIdAsync(Guid tenantId, params string[] includes);

    Task<Response<IEnumerable<RentToGetDto>>> GetTenantRentsAsync(Guid tenantId, params string[] includes);

    Task<Response<IEnumerable<BillToGetDto>>> GetTenantBillsAsync(Guid tenantId, params string[] includes);

    Task<Response<IEnumerable<PaymentToGetDto>>> GetTenantPaymentsAsync(Guid tenantId, params string[] includes);

    Task<Response<Guid>> CreateTenantAsync(TenantToCreateDto tenant);

    Task<Response<Guid>> CreateRentAsync(RentToCreateDto rent);

    Task<Response<EntityEntry<Tenant>>> UpdateTenantAsync(TenantToGetDto newTenant);

    Task<Response<EntityEntry<Tenant>>> DeleteTenantAsync(Guid tenantId);

    Task<Response<EntityEntry<Model.Library.Rent>>> CancelRentAsync(Guid rentId);

    Task<Response<Guid>> CreatePaymentAsync(PaymentToCreateDto payment);
}