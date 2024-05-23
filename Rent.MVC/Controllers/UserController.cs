using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rent.Auth.BLL.Services.Contracts;
using Rent.Auth.DAL.AuthModels;

namespace Rent.MVC.Controllers
{
    public class UserController(IUserService userService) : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignUpAction(SignUpUser user)
        {
            if (!TryValidateModel(user))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = await userService.SignUpAsync(user);

            if (!result.Exceptions.IsNullOrEmpty())
            {
                return StatusCode(500, result.Exceptions);
            }

            return new NoContentResult();
        }

        [HttpGet]
        public async Task<IActionResult> LoginAction(SignInUser signInUser)
        {
            if (!TryValidateModel(signInUser))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = await userService.LoginAsync(signInUser);

            if (!result.Exceptions.IsNullOrEmpty())
            {
                return Unauthorized("Email or password entered incorrectly");
            }

            return Ok(result);
        }
    }
}
