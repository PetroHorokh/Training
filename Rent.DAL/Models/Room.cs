using temp;

namespace Rent.DAL.Models;

public partial class Room
{
    public Guid RoomId { get; set; }

    public int Number { get; set; }

    public decimal Area { get; set; }

    public Guid AddressId { get; set; }

    public int RoomTypeId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual ICollection<AccommodationRoom> AccommodationRooms { get; set; } = new List<AccommodationRoom>();

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();

    public virtual RoomType RoomType { get; set; } = null!;
}
