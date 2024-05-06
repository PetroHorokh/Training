using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RequestsAndResponses;

namespace Rent.BLL.Services.Contracts;

public interface ITenantService
{
    Task<GetMultipleResponse<TenantToGetDto>> GetAllTenantsAsync(params string[] includes);

    Task<GetMultipleResponse<TenantToGetDto>> GetTenantsPartialAsync(GetPartialRequest request, params string[] includes);

    Task<GetMultipleResponse<TenantToGetDto>> GetFilterTenantsAsync(GetFilteredRequest filter, params string[] includes);

    Task<GetMultipleResponse<BillToGetDto>> GetAllBillsAsync(params string[] includes);

    Task<GetMultipleResponse<RentToGetDto>> GetAllRentsAsync(params string[] includes);

    Task<GetSingleResponse<TenantToGetDto>> GetTenantByIdAsync(Guid tenantId, params string[] includes);

    Task<GetMultipleResponse<RentToGetDto>> GetTenantRentsAsync(Guid tenantId, params string[] includes);

    Task<GetMultipleResponse<BillToGetDto>> GetTenantBillsAsync(Guid tenantId, params string[] includes);

    Task<GetMultipleResponse<PaymentToGetDto>> GetTenantPaymentsAsync(Guid tenantId, params string[] includes);

    Task<CreationResponse> CreateTenantAsync(TenantToCreateDto tenant);

    Task<CreationResponse> CreateRentAsync(RentToCreateDto rent);

    Task<ModifyResponse<Tenant>> UpdateTenantAsync(TenantToGetDto newTenant);

    Task<ModifyResponse<Tenant>> DeleteTenantAsync(Guid tenantId);

    Task<ModifyResponse<DAL.Models.Rent>> CancelRentAsync(Guid rentId);

    Task<CreationResponse> CreatePaymentAsync(PaymentToCreateDto payment);
}