using System;
using System.Collections.Generic;

namespace Rent.Model.Library;

public partial class VwRoomsWithTenant
{
    public int Number { get; set; }

    public string? Name { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}