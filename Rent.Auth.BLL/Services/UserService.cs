﻿using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rent.Auth.BLL.Services.Contracts;
using Rent.Auth.DAL.AuthModels;
using Rent.Auth.DAL.CustomExceptions;
using Rent.Auth.DAL.Models;
using Rent.Auth.DAL.RequestsAndResponses;

namespace Rent.Auth.BLL.Services;

public class UserService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration config) : IUserService
{
    public async Task<GetSingleResponse<AuthToken>> LoginAsync(SignInUser signInModel)
    {
        var result = new GetSingleResponse<AuthToken>();

        try
        {
            var userByEmail = await userManager.FindByEmailAsync(signInModel.Email);

            if (userByEmail is null)
            {
                throw new NoEntitiesException("No user with given email was found.");
            }

            var response1 =
                await signInManager.PasswordSignInAsync(userByEmail!.UserName!, signInModel.Password, false, false);

            if (!response1.Succeeded)
            {
                throw new CredentialValidationException("Wrong password.");
            }

            var claims = await GenerateClaims(userByEmail);

            if (claims.Error is not null)
            {
                throw claims.Error;
            }

            var accessToken = CreateAccessToken(claims.Entity!);

            if (accessToken.Error is not null)
            {
                throw accessToken.Error;
            }

            var refreshToken = GenerateRefreshToken();

            if (refreshToken.Error is not null)
            {
                throw refreshToken.Error;
            }

            userByEmail.RefreshToken = refreshToken.Entity;
            userByEmail.RefreshTokenExpiration =
                DateTime.UtcNow.AddDays(Convert.ToInt16(config["JWT:RefreshTokenExpirationDays"]));

            var response2 = await userManager.UpdateAsync(userByEmail);

            if (!response2.Errors.IsNullOrEmpty())
            {
                throw new IdentityException(response2.Errors);
            }

            result.Entity = new AuthToken()
            {
                AccessToken = accessToken.Entity!,
                RefreshToken = refreshToken.Entity!
            };
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public Task Logout()
    {
        return signInManager.SignOutAsync();
    }

    public async Task<GetSingleResponse<IdentityResult>> SignUpAsync(SignUpUser signUpModel)
    {
        var result = new GetSingleResponse<IdentityResult>();

        try
        {
            var user = new User()
            {
                Email = signUpModel.Email,
                UserName = signUpModel.Login,
                PhoneNumber = signUpModel.PhoneNumber,
            };

            var response1 = await userManager.CreateAsync(user, signUpModel.Password);
            if (response1.Succeeded)
            {
                var response2 = await userManager.AddToRoleAsync(user, "User");

                if (!response2.Succeeded)
                {
                    throw new ProcessException("Could not assign role for user");
                }

                var response3 = await userManager.UpdateAsync(user);

                if (!response3.Succeeded)
                {
                    throw new ProcessException("Could not update user with added roles");
                }
            }
            else
            {
                throw new ProcessException("Something went wrong while creating new user");
            }

            result.Entity = response1;
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public async Task<GetSingleResponse<string>> RenewAccessToken(AuthToken token)
    {
        var result = new GetSingleResponse<string>();

        try
        {
            var principal = GetPrincipalFromAccessToken(token.AccessToken);

            if (principal.Error is not null)
            {
                throw principal.Error;
            }

            string username = principal.Entity!.Identity!.Name!;

            var user = await userManager.FindByNameAsync(username);

            if (user is null)
            {
                throw new NoEntitiesException("Could not find user.");
            }

            if (user.RefreshToken != token.RefreshToken)
            {
                throw new CredentialValidationException("Refresh token is not recognised.");
            }

            if (user.RefreshTokenExpiration <= DateTime.Now)
            {
                throw new ValidationException("Refresh token is not valid due to expiration");
            }

            var claims = await GenerateClaims(user);

            if (claims.Error is not null)
            {
                throw claims.Error;
            }

            var accessToken = CreateAccessToken(claims.Entity!);

            if (accessToken.Error is not null)
            {
                throw accessToken.Error;
            }

            result.Entity = accessToken.Entity;
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public async Task<GetSingleResponse<IdentityResult>> ChangePasswordAsync(PasswordChange changePassword)
    {
        var result = new GetSingleResponse<IdentityResult>();

        try
        {
            var user = await userManager.FindByEmailAsync(changePassword.Email);

            if (user is null)
            {
                throw new NoEntitiesException("No user with given email was found.");
            }

            var response = await userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);

            if (!response.Errors.IsNullOrEmpty())
            {
                throw new IdentityException(response.Errors);
            }

            result.Entity = response;
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    public async Task<GetSingleResponse<IdentityResult>> ChangeEmailAsync(EmailChange emailChange)
    {
        var result = new GetSingleResponse<IdentityResult>();

        try
        {
            var user = await userManager.FindByEmailAsync(emailChange.CurrentEmail);

            if (user is null)
            {
                throw new NoEntitiesException("No user with given email was found.");
            }

            var token = await userManager.GenerateChangeEmailTokenAsync(user, emailChange.NewEmail);

            var response = await userManager.ChangeEmailAsync(user, emailChange.NewEmail, token);

            if (!response.Errors.IsNullOrEmpty())
            {
                throw new IdentityException(response.Errors);
            }

            result.Entity = response;
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    private GetSingleResponse<ClaimsPrincipal> GetPrincipalFromAccessToken(string token)
    {
        var result = new GetSingleResponse<ClaimsPrincipal>();

        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]!)),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            result.Entity = principal;
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    private GetSingleResponse<string> CreateAccessToken(ClaimsIdentity claims)
    {
        var result = new GetSingleResponse<string>();

        try
        {
            var handler = new JwtSecurityTokenHandler();

            var privateKey = Encoding.ASCII.GetBytes(config["JWT:Secret"]!);

            var credentials =
                new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(Convert.ToInt16(config["JWT:AccessTokenExpirationHours"])),
                Subject = claims,
                Issuer = config["JWT:ValidIssuer"],
                Audience = config["JWT:ValidAudience"]
            };

            var token = handler.CreateToken(tokenDescriptor);
            result.Entity = handler.WriteToken(token);
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    private GetSingleResponse<string> GenerateRefreshToken()
    {
        var result = new GetSingleResponse<string>();

        try
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            result.Entity = Convert.ToBase64String(randomNumber);
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }

    private async Task<GetSingleResponse<ClaimsIdentity>> GenerateClaims(User user)
    {
        var result = new GetSingleResponse<ClaimsIdentity>();

        try
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName!));
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email!));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
                claims.AddClaim(new Claim(ClaimTypes.Role, role));

            result.Entity = claims;
        }
        catch (Exception ex)
        {
            result.Error = ex;
        }

        return result;
    }
}