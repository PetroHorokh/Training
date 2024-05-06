using System.ComponentModel.DataAnnotations;

namespace Rent.Auth.DAL.AuthModels;

/// <summary>
/// Class for sign up of user.
/// </summary>
public class SignUpUser
{
    /// <summary>
    /// Property containing login of new user.
    /// </summary>
    [Required(ErrorMessage = "Login is required")]
    [RegularExpression(@"^[a-zA-Z0-9_.-]*$", ErrorMessage = "Login is invalid")]
    public string Login { get; set; } = null!;

    /// <summary>
    /// Property containing email of new user.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[\d\w._-]+@[\d\w._-]+\.[\w]+$", ErrorMessage = "Email is invalid")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Property containing password of new user.
    /// </summary>
    [Required]
    [Compare("ConfirmPassword")]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "Password should be at least 8 symbols", MinimumLength = 8)]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Property containing confirm password of new user.
    /// </summary>
    /// <remarks>Should be equal to password</remarks>
    [Required]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

    /// <summary>
    /// Property containing phone number of new user.
    /// </summary>
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;
}