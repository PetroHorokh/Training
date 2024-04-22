using AutoMapper;
using Rent.DAL.DTO;
using Rent.DAL.Models;

namespace Rent.BLL.Profiles;

public class TenantProfile : MappingProfile
{
    public TenantProfile()
    {
        CreateMap<Tenant, TenantToGetDto>()
            .ForMember(x => x.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(x => x.Director, opt => opt.MapFrom(s => s.Director))
            .ForMember(x => x.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(x => x.BankName, opt => opt.MapFrom(s => s.BankName))
            .ForMember(x => x.AddressId, opt => opt.MapFrom(s => s.AddressId))
            .ForMember(x => x.TenantId, opt => opt.MapFrom(s => s.TenantId));

        //CreateMap<TenantToCreateDto, TenantToUpdateDto>();

        CreateMap<DAL.Models.Rent, RentToGetDto>();
    }
}