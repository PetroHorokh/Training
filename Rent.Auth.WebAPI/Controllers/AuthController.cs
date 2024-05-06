using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Auth.BLL.Services.Contracts;
using Rent.Auth.DAL.AuthModels;

namespace Rent.Auth.WebAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController(IUserService userService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] SignUpUser user)
    {
        var result = await userService.SignUpAsync(user);

        if (result.Error is not null)
        {
            throw result.Error;
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> RenewAccessToken([FromHeader] AuthToken token)
    {
        var result = await userService.RenewAccessToken(token);

        if (result.Error is not null)
        {
            throw result.Error;
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Login([FromQuery] SignInUser signInUser)
    {
        var result = await userService.LoginAsync(signInUser);

        if (result.Error is not null)
        {
            throw result.Error;
        }

        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await userService.Logout();
        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangeEmail([FromBody] EmailChange emailChange)
    {
        var result = await userService.ChangeEmailAsync(emailChange);

        if (result.Error is not null)
        {
            throw result.Error;
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChange passwordChange)
    {
        var result = await userService.ChangePasswordAsync(passwordChange);

        if (result.Error is not null)
        {
            throw result.Error;
        }

        return Ok(result);
    }
}