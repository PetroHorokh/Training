namespace Rent.DAL.DTO;

public class RentToGetDto
{
    public Guid RentId { get; set; }

    public Guid AssetId { get; set; }

    public Guid TenantId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public override string ToString() => $"\nRent with id {RentId} information\nAsset id = {AssetId}\nTenant id = {TenantId}\nStart date = {StartDate}\nEnd date = {EndDate}";
}