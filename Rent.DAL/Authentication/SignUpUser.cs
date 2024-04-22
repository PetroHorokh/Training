using System.ComponentModel.DataAnnotations;

namespace Rent.DAL.Authentication;

public class SignUpUser
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[\d\w._-]+@[\d\w._-]+\.[\w]+$", ErrorMessage = "Email is invalid")]
    public string Email { get; set; } = null!;

    [Required]
    [Compare("ConfirmPassword")]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "Password should be at least 8 symbols", MinimumLength = 8)]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;
}