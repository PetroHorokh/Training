using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rent.DTOs.Library;

public class RentToCreateDto
{
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
    public DateTime EndDate { get; set; }
}