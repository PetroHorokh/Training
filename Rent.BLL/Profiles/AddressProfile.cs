using AutoMapper;
using Rent.DAL.DTO;
using Rent.DAL.Models;

namespace Rent.BLL.Profiles;

public class AddressProfile : MappingProfile
{
    public AddressProfile()
    {
        CreateMap<Address, AddressToGetDto>();
    }
}