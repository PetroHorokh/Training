using Amazon.DynamoDBv2.DataModel;

namespace Rent.AWS.Lambda.TenantBillGetter.Models;

[DynamoDBTable("TenantBills")]
public class Bill
{
    [DynamoDBHashKey]
    public Guid BillId { get; set; }

    [DynamoDBRangeKey]
    public Guid TenantId { get; set; }

    public Guid RentId { get; set; }

    public decimal BillAmount { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? EndDate { get; set; }
}