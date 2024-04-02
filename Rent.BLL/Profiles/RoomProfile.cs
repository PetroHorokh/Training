using Rent.DAL.DTO;
using Rent.DAL.Models;

namespace Rent.BLL.Profiles;

public class RoomProfile : MappingProfile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomToGetDto>();

        CreateMap<RoomType, RoomTypeToGetDto>();

        CreateMap<Accommodation, AccommodationToGetDto>();

        CreateMap<AccommodationRoom, AccommodationRoomToGetDto>()
            .ForMember(x => x.AccommodationId, opt => opt.MapFrom(s => s.AccommodationId))
            .ForMember(x => x.Name, opt => opt.MapFrom(s => s.Accommodation.Name))
            .ForMember(x => x.AccommodationId, opt => opt.MapFrom(s => s.AccommodationId))
            .ForMember(x => x.RoomId, opt => opt.MapFrom(s => s.RoomId))
            .ForMember(x => x.Quantity, opt => opt.MapFrom(s => s.Quantity));
    }
}