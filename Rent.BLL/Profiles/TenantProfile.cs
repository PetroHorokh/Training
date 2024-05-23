using AutoMapper;
using Rent.DTOs.Library;
using Rent.Model.Library;

namespace Rent.BLL.Profiles;

public class TenantProfile : MappingProfile
{
    public TenantProfile()
    {
        CreateMap<Tenant, TenantToGetDto>();

        //CreateMap<TenantToCreateDto, TenantToUpdateDto>();

        CreateMap<Model.Library.Rent, RentToGetDto>();
    }
}