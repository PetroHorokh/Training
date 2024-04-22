using System.ComponentModel.DataAnnotations;

namespace Rent.DAL.Authentication;

public class SignInUser
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[\d\w._-]+@[\d\w._-]+\.[\w]+$", ErrorMessage = "Email is invalid")]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}