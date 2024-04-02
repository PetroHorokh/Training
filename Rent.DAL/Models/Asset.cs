﻿namespace Rent.DAL.Models;

public partial class Asset
{
    public Guid AssetId { get; set; }

    public Guid OwnerId { get; set; }

    public Guid RoomId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public virtual Owner Owner { get; set; } = null!;

    public virtual ICollection<Rent> Rents { get; set; } = [];

    public virtual Room Room { get; set; } = null!;
}
