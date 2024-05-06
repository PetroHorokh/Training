namespace Rent.Auth.DAL.Models;

public class Tenant
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string Director { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}
