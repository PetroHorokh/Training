using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rent.Auth.BLL.Services.Contracts;
using Rent.Auth.DAL.AuthModels;
using Rent.BLL.Services;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;

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

            try
            {
                var result = await userService.SignUpAsync(user);

                if (result.Error is null)
                {
                    return StatusCode(201, result);
                }
                else
                {
                    return StatusCode(409, result.Error.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoginAction(SignInUser signInUser)
        {
            if (!TryValidateModel(signInUser))
                return BadRequest(GetFullErrorMessage(ModelState));

            try
            {
                var result = await userService.LoginAsync(signInUser);

                if (result is null)
                {
                    return Unauthorized("Email or password entered incorrectly");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
