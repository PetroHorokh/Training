using AutoMapper;
using Rent.DTOs.Library;
using Rent.Model.Library;

namespace Rent.BLL.Profiles;

public class AddressProfile : MappingProfile
{
    public AddressProfile()
    {
        CreateMap<Address, AddressToGetDto>();
    }
}