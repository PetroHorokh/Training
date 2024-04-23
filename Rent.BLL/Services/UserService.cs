using AutoMapper;
using Microsoft.Extensions.Logging;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Rent.DAL.Authentication;
using Rent.DAL.RequestsAndResponses;

namespace Rent.BLL.Services;

public class UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger, IConfiguration config) : IUserService
{
    public async Task<AuthToken?> LoginAsync(SignInUser signInModel)
    {
        var response = await unitOfWork.Users.GetSingleByConditionAsync(user => user.Email == signInModel.Email);
        var userByEmail = response.Entity;
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

    public async Task<GetSingleResponse<User>> GetUserByIdAsync(Guid userId)
    {
        var result = new GetSingleResponse<User>();

        try
        {
            logger.LogInformation("Entering UserService, GetUserByIdAsync");

            logger.LogInformation("Calling UserRepository, method GetSingleByConditionAsync");
            var response = await unitOfWork.Users.GetSingleByConditionAsync(user => user.UserId == userId);
            logger.LogInformation("Finished calling UserRepository, method GetSingleByConditionAsync");

            result.TimeStamp = response.TimeStamp;
            if (response.Error is not null)
            {
                throw response.Error;
            }

            logger.LogInformation("Exiting UserService, GetUserByIdAsync");
            return result;
        }
        catch (Exception ex)
        {
            result.Error = ex;
            return result;
        }
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

        var response1 = await unitOfWork.UserRoles.GetByConditionAsync(userRole => userRole.UserId == user.UserId);
        var response2 = await unitOfWork.Roles.GetAllAsync();

        var roles = response2.Collection!.Join(response1.Collection!, l => l.RoleId, userRole => userRole.RoleId, (l, userRole) => new
        {
            RoleId = l.RoleId,
            Name = l.Name
        });

        foreach (var role in roles)
            cl.AddClaim(new Claim(ClaimTypes.Role, role.Name));

        return cl;
    }
}