namespace Rent.DAL.DTO;

public class RentToCreateDto
{
    public Guid AssetId { get; set; }

    public Guid TenantId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}