using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Rent.Auth.DAL.Models;
using Rent.DAL.Models;

namespace Rent.MVC.Helpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User?)context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}