using AutoMapper;
using Microsoft.Extensions.Logging;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Responses;
using Rent.DAL.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Rent.DAL.Authentication;

namespace Rent.BLL.Services;

public class UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger, IConfiguration config) : IUserService
{
    public async Task<AuthToken?> LoginAsync(SignInUser signInModel)
    {
        var userByEmail = await unitOfWork.Users.GetSingleByConditionAsync(user => user.Email == signInModel.Email);
        if (userByEmail is null) return null;
        
        if (signInModel.Password != userByEmail.Password) return null;

        var token = await CreateToken(userByEmail);

        return new AuthToken
        {
            Token = token
        };
    }

    public async Task<CreationResponse> SignUpAsync(SignUpUser signUpModel)
    {
        string name = signUpModel.Email.Substring(0, signUpModel.Email.IndexOf("@", StringComparison.Ordinal));

        var user = new UserToCreateDto()
        {
            Email = signUpModel.Email,
            Name = name,
            Password = signUpModel.Password,
            PhoneNumber = signUpModel.PhoneNumber
        };

        var result = await unitOfWork.Users.CreateWithProcedure(user);

        return result;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        logger.LogInformation("Entering UserService, GetUserByIdAsync");

        logger.LogInformation("Calling UserRepository, method GetSingleByConditionAsync");
        logger.LogInformation($"Parameter: userId = {userId}");
        var user = await unitOfWork.Users.GetSingleByConditionAsync(user => user.UserId == userId);
        logger.LogInformation("Finished calling UserRepository, method GetByConditionAsync");

        logger.LogInformation("Exiting UserService, GetUserByIdAsync");
        return user;
    }

    private async Task<string> CreateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        var privateKey = Encoding.ASCII.GetBytes(config["JWT:Secret"]!);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(privateKey),
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(1),
            Subject = await GenerateClaims(user)
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private async Task<ClaimsIdentity> GenerateClaims(User user)
    {
        var cl = new ClaimsIdentity();

        cl.AddClaim(new Claim("UserId", user.UserId.ToString()));
        cl.AddClaim(new Claim(ClaimTypes.Name, user.Name));
        cl.AddClaim(new Claim(ClaimTypes.Email, user.Email));

        var userRoles = await unitOfWork.UserRoles.GetByConditionAsync(userRole => userRole.UserId == user.UserId);
        var roles = (await unitOfWork.Roles.GetAllAsync()).Join(userRoles, l => l.RoleId, userRole => userRole.RoleId, (l, userRole) => new
        {
            RoleId = l.RoleId,
            Name = l.Name
        });

        foreach (var role in roles)
            cl.AddClaim(new Claim(ClaimTypes.Role, role.Name));

        return cl;
    }
}