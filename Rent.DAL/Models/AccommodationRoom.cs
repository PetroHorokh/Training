namespace Rent.DAL.Models;

public partial class AccommodationRoom
{
    public Guid AccommodationRoomId { get; set; }

    public int AccommodationId { get; set; }

    public Guid RoomId { get; set; }

    public int Quantity { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual Accommodation Accommodation { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
