namespace Rent.Auth.DAL.AuthModels;

/// <summary>
/// Class for working with combination of access and refresh tokens.
/// </summary>
public class AuthToken
{
    /// <summary>
    /// Property containing access token.
    /// </summary>
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// Property containing refresh token.
    /// </summary>
    public string RefreshToken { get; set; } = null!;
}