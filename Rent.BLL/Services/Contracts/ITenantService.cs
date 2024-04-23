using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.RequestsAndResponses;

namespace Rent.BLL.Services.Contracts;

public interface ITenantService
{
    Task<GetMultipleResponse<TenantToGetDto>> GetAllTenantsAsync();

    Task<GetMultipleResponse<TenantToGetDto>> GetTenantsPartialAsync(GetPartialRequest request);

    Task<GetMultipleResponse<BillToGetDto>> GetAllBillsAsync();

    Task<GetMultipleResponse<RentToGetDto>> GetAllRentsAsync();

    Task<GetSingleResponse<TenantToGetDto>> GetTenantByIdAsync(Guid tenantId);

    Task<GetMultipleResponse<RentToGetDto>> GetTenantRentsAsync(Guid tenantId);

    Task<GetMultipleResponse<BillToGetDto>> GetTenantBillsAsync(Guid tenantId);

    Task<GetMultipleResponse<PaymentToGetDto>> GetTenantPaymentsAsync(Guid tenantId);

    Task<CreationResponse> CreateTenantAsync(TenantToCreateDto tenant);

    Task<CreationResponse> CreateRentAsync(RentToCreateDto rent);

    Task<ModifyResponse<Tenant>> UpdateTenantAsync(TenantToGetDto newTenant);

    Task<ModifyResponse<Tenant>> DeleteTenantAsync(Guid tenantId);

    Task<ModifyResponse<DAL.Models.Rent>> CancelRentAsync(Guid rentId);

    Task<CreationResponse> CreatePaymentAsync(PaymentToCreateDto payment);
}