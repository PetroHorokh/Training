using Rent.DTOs.Library;
using Rent.Model.Library;

namespace Rent.BLL.Profiles;

public class AssetProfile : MappingProfile
{
    public AssetProfile()
    {
        CreateMap<Asset, AssetToGetDto>();
    }
}