using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.DTOs.Library;

public class TenantToCreateDto
{
    [Required]
    [Column(TypeName = "nvarchar(255)")]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(255)")]
    [Display(Name = "Bank name")]
    public string BankName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(255)")]
    [RegularExpression(@"\b[A-Z][a-zA-Z]*\s[A-Z][a-zA-Z]*\b", ErrorMessage = @"Directors name should be in format ""Lastname Firstname""")]
    public string Director { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(255)")]
    public string Description { get; set; } = null!;

    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid AddressId { get; set; }
}