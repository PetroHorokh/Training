using System.ComponentModel.DataAnnotations;

namespace Rent.Auth.DAL.AuthModels;

/// <summary>
/// Class for login of user.
/// </summary>
public class SignInUser
{
    /// <summary>
    /// Property containing email.
    /// </summary>
    [Required]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[\d\w._-]+@[\d\w._-]+\.[\w]+$", ErrorMessage = "Email is invalid")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Property containing password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}