namespace Rent.ADO.NET.DTOs;

public class AddressDto
{
    public Guid AddressId { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Building { get; set; } = null!;

    public override string ToString() => $"\nAddress with id {AddressId} information\nCity: {City}\nStreet: {Street}\nBuilding: {Building}";
}