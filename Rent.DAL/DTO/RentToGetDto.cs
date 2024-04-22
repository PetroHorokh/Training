using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rent.DAL.DTO;

public class RentToGetDto
{
    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid RentId { get; set; }

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid AssetId { get; set; }

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid TenantId { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime StartDate { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime? EndDate { get; set; }

    public override string ToString() => $"\nRent with id {RentId} information\nAsset id = {AssetId}\nTenant id = {TenantId}\nStart date = {StartDate}\nEnd date = {EndDate}";
}