using Rent.DTOs.Library;
using Rent.Model.Library;

namespace Rent.BLL.Profiles;

public class OwnerProfile : MappingProfile
{
    public OwnerProfile()
    {
        CreateMap<Owner, OwnerToGetDto>();
    }
}