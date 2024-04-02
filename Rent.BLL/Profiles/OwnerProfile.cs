using Rent.DAL.DTO;
using Rent.DAL.Models;

namespace Rent.BLL.Profiles;

public class OwnerProfile : MappingProfile
{
    public OwnerProfile()
    {
        CreateMap<Owner, OwnerToGetDto>();
    }
}