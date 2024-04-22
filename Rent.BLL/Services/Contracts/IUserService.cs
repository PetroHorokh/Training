using Microsoft.AspNetCore.Mvc;
using Rent.DAL.Authentication;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Responses;

namespace Rent.BLL.Services.Contracts;

public interface IUserService
{
    Task<AuthToken?> LoginAsync(SignInUser signInModel);

    Task<CreationResponse> SignUpAsync(SignUpUser signUpModel);

    Task<User?> GetUserByIdAsync(Guid userId);
}