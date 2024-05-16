using Microsoft.AspNetCore.Identity;
using Rent.Auth.DAL.AuthModels;
using Rent.Auth.DAL.Models;
using Rent.Auth.DAL.RequestsAndResponses;

namespace Rent.Auth.BLL.Services.Contracts;

public interface IUserService
{
    Task<GetSingleResponse<AuthToken>> LoginAsync(SignInUser signInModel);

    Task Logout();

    Task<GetSingleResponse<IdentityResult>> SignUpAsync(SignUpUser signUpModel);

    Task<GetSingleResponse<string>> RenewAccessToken(AuthToken token);

    Task<GetSingleResponse<IdentityResult>> ChangePasswordAsync(PasswordChange changePassword);

    Task<GetSingleResponse<IdentityResult>> ChangeEmailAsync(EmailChange emailChange);

    Task<GetSingleResponse<ModifyResponse<Image>>> PostImage(PostImageRequest request);
}