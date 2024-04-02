namespace Rent.DAL.Models;

public partial class Price
{
    public Guid PriceId { get; set; }

    public DateTime StartDate { get; set; }

    public decimal Value { get; set; }

    public DateTime? EndDate { get; set; }

    public int RoomTypeId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual RoomType RoomType { get; set; } = null!;
}
