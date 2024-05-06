using System.ComponentModel.DataAnnotations;

namespace Rent.Auth.DAL.AuthModels;

/// <summary>
/// Class for changing email of user.
/// </summary>
public class EmailChange
{
    /// <summary>
    /// Property containing current email.
    /// </summary>
    [Required]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[\d\w._-]+@[\d\w._-]+\.[\w]+$", ErrorMessage = "Email is invalid")]
    public string CurrentEmail { get; set; } = null!;

    /// <summary>
    /// Property containing new email.
    /// </summary>
    [Required]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[\d\w._-]+@[\d\w._-]+\.[\w]+$", ErrorMessage = "Email is invalid")]
    public string NewEmail { get; set; } = null!;

    /// <summary>
    /// Property containing password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}