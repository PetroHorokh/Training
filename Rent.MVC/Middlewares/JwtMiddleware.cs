using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rent.BLL.Services.Contracts;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Rent.MVC.Middlewares;

public class JwtMiddleware(RequestDelegate next, IConfiguration config)
{
    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            await AttachUserToContext(context, userService, token);

        await next(context);
    }

    private async Task AttachUserToContext(HttpContext context, IUserService userService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config["JWT:Secret"]!);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

            context.Items["User"] = await userService.GetUserByIdAsync(userId);
        }
        catch
        {
            //Do nothing if JWT validation fails
            // user is not attached to context so the request won't have access to secure routes
        }
    }
}
