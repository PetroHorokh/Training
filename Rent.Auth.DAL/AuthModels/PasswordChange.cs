using System.ComponentModel.DataAnnotations;

namespace Rent.Auth.DAL.AuthModels;

/// <summary>
/// Class for changing password of user.
/// </summary>
public class PasswordChange
{
    /// <summary>
    /// Property containing current email.
    /// </summary>
    [Required]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[\d\w._-]+@[\d\w._-]+\.[\w]+$", ErrorMessage = "Email is invalid")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Property containing current password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;

    /// <summary>
    /// Property containing new password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = null!;
}