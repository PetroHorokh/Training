using System;
using System.Collections.Generic;

namespace Rent.Model.Library;

public partial class VwCertificateForTenant
{
    public Guid TenantId { get; set; }

    public Guid? RentId { get; set; }

    public DateTime? RentStartDate { get; set; }

    public DateTime? RentEndDate { get; set; }

    public Guid? BillId { get; set; }

    public Guid? PaymentId { get; set; }
}