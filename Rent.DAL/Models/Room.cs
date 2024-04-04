namespace Rent.DAL.Models;

public partial class Room
{
    public Guid RoomId { get; set; }

    public int Number { get; set; }

    public decimal Area { get; set; }

    public int RoomTypeId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual ICollection<AccommodationRoom> AccommodationRooms { get; set; } = [];

    public virtual ICollection<Asset> Assets { get; set; } = [];

    public virtual RoomType RoomType { get; set; } = null!;
}
