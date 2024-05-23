using Rent.DTOs.Library;
using Rent.Model.Library;

namespace Rent.BLL.Profiles;

public class ViewProfile : MappingProfile
{
    public ViewProfile()
    {
        CreateMap<VwTenantAssetPayment, VwTenantAssetPaymentToGetDto>();
        CreateMap<VwRoomsWithTenant, VwRoomsWithTenantToGetDto>();
    }
}