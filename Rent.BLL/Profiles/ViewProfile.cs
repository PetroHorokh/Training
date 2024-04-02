using Rent.DAL.DTO;
using temp;

namespace Rent.BLL.Profiles;

public class ViewProfile : MappingProfile
{
    public ViewProfile()
    {
        CreateMap<VwTenantAssetPayment, VwTenantAssetPaymentToGetDto>();
        CreateMap<VwRoomsWithTenant, VwRoomsWithTenantToGetDto>();
    }
}