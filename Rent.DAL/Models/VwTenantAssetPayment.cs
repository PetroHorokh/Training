using System;
using System.Collections.Generic;

namespace temp;

public partial class VwTenantAssetPayment
{
    public Guid? TenantId { get; set; }

    public string? Name { get; set; }

    public Guid? RentId { get; set; }

    public int Number { get; set; }

    public decimal? Price { get; set; }
}