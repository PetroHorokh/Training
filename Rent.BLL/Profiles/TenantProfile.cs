using AutoMapper;
using Rent.DAL.DTO;
using Rent.DAL.Models;

namespace Rent.BLL.Profiles;

public class TenantProfile : MappingProfile
{
    public TenantProfile()
    {
        CreateMap<Tenant, TenantToGetDto>();

        CreateMap<TenantToCreateDto, TenantToUpdateDto>();

        CreateMap<DAL.Models.Rent, RentToGetDto>();
    }
}