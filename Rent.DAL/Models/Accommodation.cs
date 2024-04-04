namespace Rent.DAL.Models;

public partial class Accommodation
{
    public int AccommodationId { get; set; }

    public string Name { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual ICollection<AccommodationRoom> AccommodationRooms { get; set; } = [];
}
