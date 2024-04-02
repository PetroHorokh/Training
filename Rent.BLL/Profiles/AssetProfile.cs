using Rent.DAL.DTO;
using Rent.DAL.Models;

namespace Rent.BLL.Profiles;

public class AssetProfile : MappingProfile
{
    public AssetProfile()
    {
        CreateMap<Asset, AssetToGetDto>();
    }
}