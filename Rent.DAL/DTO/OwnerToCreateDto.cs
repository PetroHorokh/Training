namespace Rent.DAL.DTO;

public class OwnerToCreateDto
{
    public string Name { get; set; } = null!;

    public Guid AddressId { get; set; }
}