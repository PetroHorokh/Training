using System.ComponentModel.DataAnnotations;

namespace Rent.DAL.DTO;

public class UserToCreateDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string PhoneNumber { get; set; } = null!;
}