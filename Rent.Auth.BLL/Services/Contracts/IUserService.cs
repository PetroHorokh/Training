using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rent.Auth.DAL.AuthModels;
using Rent.Auth.DAL.Models;
using Rent.ResponseAndRequestLibrary;

namespace Rent.Auth.BLL.Services.Contracts;

public interface IUserService
{
    Task<Response<AuthToken>> LoginAsync(SignInUser signInModel);

    Task Logout();

    Task<Response<IdentityResult>> SignUpAsync(SignUpUser signUpModel);

    Task<Response<string>> RenewAccessToken(AuthToken token);

    Task<Response<IdentityResult>> ChangePasswordAsync(PasswordChange changePassword);

    Task<Response<IdentityResult>> ChangeEmailAsync(EmailChange emailChange);

    Task<Response<EntityEntry<Image>>> PostImage(PostImageRequest request);
}