using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Auth.BLL.Services.Contracts;
using Rent.Auth.DAL.AuthModels;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Rent.AWS.S3.Services.Contracts;
using Rent.ExceptionLibrary;
using Rent.ResponseAndRequestLibrary;


namespace Rent.Auth.WebAPI.Controllers;

/// <summary>
/// 
/// </summary>
/// <param name="userService"></param>
/// <param name="s3Service"></param>
[ApiController]
[Route("[controller]/[action]")]
public class AuthController(IUserService userService, IS3Service s3Service) : Controller
{
    /// <summary>
    /// Sign up user
    /// </summary>
    /// <param name="user">Parameter to sign up new user by <see cref="SignUpUser"/> model</param>
    /// <returns>Return status of creation of new user</returns>
    /// <exception cref="ProcessException">Thrown when an error occured in user service</exception>
    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] SignUpUser user)
    {
        var result = await userService.SignUpAsync(user);

        if (!result.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, result.Exceptions);
        }

        return NoContent();
    }

    /// <summary>
    /// Renew access token based on old one and refresh token
    /// </summary>
    /// <param name="token">Parameter to pass old access token and refresh token</param>
    /// <returns>Return <see cref="string"/> new access token</returns>
    ///// <exception cref="ProcessException">Thrown when an error occured in user service</exception>
    [HttpGet]
    public async Task<IActionResult> RenewAccessToken([FromQuery] AuthToken token)
    {
        var result = await userService.RenewAccessToken(token);

        if (!result.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, result.Exceptions);
        }

        return Ok(result);
    }

    /// <summary>
    /// Sign in user
    /// </summary>
    /// <param name="signInUser">Parameter to login user by <see cref="SignInUser"/> model</param>
    /// <returns>Return <see cref="AuthToken"/> model with access and refresh tokens</returns>
    /// <exception cref="ProcessException">Thrown when an error occured in user service</exception>
    [HttpGet]
    public async Task<IActionResult> Login([FromQuery] SignInUser signInUser)
    {
        var result = await userService.LoginAsync(signInUser);

        if (!result.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, result.Exceptions);
        }

        return Ok(result);
    }

    /// <summary>
    /// Sign out currently logged-in user
    /// </summary>
    /// <returns>No content if successful</returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await userService.Logout();
        return NoContent();
    }

    /// <summary>
    /// Change email of user to a new one
    /// </summary>
    /// <param name="emailChange">Parameter of type <see cref="EmailChange"/> to change email with current email, new email and current password</param>
    /// <returns>Return status of changing user email</returns>
    /// <exception cref="ProcessException">Thrown when an error occured in user service</exception>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeEmail([FromBody] EmailChange emailChange)
    {
        var result = await userService.ChangeEmailAsync(emailChange);

        if (!result.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, result.Exceptions);
        }

        return Ok(result);
    }

    /// <summary>
    /// Change password of user to a new one
    /// </summary>
    /// <param name="passwordChange">Parameter of type <see cref="PasswordChange"/> to change password with current password, new password and current email</param>
    /// <returns>Return status of changing user password</returns>
    /// <exception cref="ProcessException">Thrown when an error occured in user service</exception>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChange passwordChange)
    {
        var result = await userService.ChangePasswordAsync(passwordChange);

        if (!result.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, result.Exceptions);
        }

        return Ok(result);
    }

    /// <summary>
    /// Upload image to AWS S3 bucket as an avatar
    /// </summary>
    /// <param name="file">Parameter to include image</param>
    /// <returns>Return status of adding new image as an avatar</returns>
    /// <exception cref="ProcessException">Thrown when an error occured in user service</exception>
    ///// <exception cref="AmazonS3Exception">Added to response when an error occured is S3 service</exception>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result1 = await s3Service.SendFileToS3Bucket(file);

        var request = new PostImageRequest()
        {
            Image = file,
            UserId = userId,
            Url = result1.Body
        };

        var result2 = await userService.PostImage(request);

        if (!result2.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, result2.Exceptions);
        }

        if (!result1.Exceptions.IsNullOrEmpty())
        {
            return StatusCode(500, result1.Exceptions);
        }

        return NoContent();
    }
}