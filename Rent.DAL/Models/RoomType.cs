using temp;

namespace Rent.DAL.Models;

public partial class RoomType
{
    public int RoomTypeId { get; set; }

    public string Name { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual ICollection<Price> Prices { get; set; } = new List<Price>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
