using Rent.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.DAL.DTO;

public class TenantToGetDto
{
    [Required]
    [Column(TypeName = "uniqueidentifier")]
    public Guid TenantId { get; set; }

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

    public Address Address { get; set; } = null!;

    public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public ICollection<Models.Rent> Rents { get; set; } = new List<Models.Rent>();

    public override string ToString() =>
        $"\nTenant {TenantId} information\nName: {Name}\nDirector: {Director}\nDescription: {Description}\nBank name: {BankName}\nAddress id: {AddressId}";
}