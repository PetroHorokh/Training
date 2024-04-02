namespace Rent.DAL.DTO;

public class OwnerToGetDto
{
    public Guid OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public Guid AddressId { get; set; }

    public override string ToString() => $"\nOwner with id {OwnerId} information\nName: {Name}\nAddressId: {AddressId}";
}