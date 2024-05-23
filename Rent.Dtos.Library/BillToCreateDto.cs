namespace Rent.DTOs.Library;

public class BillToCreateDto
{
    public Guid TenantId { get; set; }

    public Guid AssetId { get; set; }

    public decimal BillAmount { get; set; }
}