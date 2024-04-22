using temp;

namespace Rent.DAL.Models;

public partial class UserRole
{
    public Guid UserRoleId { get; set; }

    public Guid UserId { get; set; }

    public int RoleId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
