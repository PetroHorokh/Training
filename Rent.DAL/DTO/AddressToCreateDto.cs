namespace Rent.DAL.DTO;

public class AddressToCreateDto
{
    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Building { get; set; } = null!;

    public Guid CreatedBy { get; set; }
}