using Azure;
using Rent.DAL.Models;

namespace Rent.DAL.DTO;

public class TenantToGetDto
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public Guid AddressId { get; set; }

    public string Director { get; set; } = null!;

    public string Description { get; set; } = null!;

    public override string ToString() =>
        $"\nTenant {TenantId} information\nName: {Name}\nDirector: {Director}\nDescription: {Description}\nBank name: {BankName}\nAddress id: {AddressId}";
}