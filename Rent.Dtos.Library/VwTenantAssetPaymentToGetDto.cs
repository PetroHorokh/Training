namespace Rent.DTOs.Library;

public class VwTenantAssetPaymentToGetDto
{
    public string? Name { get; set; }

    public Guid? RentId { get; set; }

    public int Number { get; set; }

    public decimal? Price { get; set; }

    public override string ToString() =>
        RentId != null
            ? $"Tenant name: {Name}\nRent id: {RentId}\nRoom number: {Number}\nPrice: {Price}\n"
            : $"Tenant does not have rents";
}