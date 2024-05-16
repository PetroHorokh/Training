using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Rent.Auth.BLL.Services;
using Rent.Auth.BLL.Services.Contracts;
using Rent.Auth.DAL.Models;
using Rent.Auth.DAL.UnitOfWork;

namespace Rent.Auth.BLL.Tests;

public class SetUp
{
    public static UserManager<User> UserManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(),
        null, null, null, null, null, null, null, null);

    public static SignInManager<User> SignInManager = Substitute.For<SignInManager<User>>(UserManager,
        Substitute.For<IHttpContextAccessor>(), Substitute.For<IUserClaimsPrincipalFactory<User>>(), null, null, null,
        null);

    public static IUnitOfWork UnitOfWork = Substitute.For<IUnitOfWork>();
    public static IConfiguration Config = Substitute.For<IConfiguration>();
    public readonly IUserService UserService = new UserService(UserManager, SignInManager, UnitOfWork, Config);
    public required IFormFile File;

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        var content = string.Empty;
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        stream.Position = 0;

        File = new FormFile(stream, 0, stream.Length, string.Empty, fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application.pdf"
        };
    }

    [OneTimeTearDown]
    public void GlobalCleanUp()
    {
        UserManager.Dispose();
        UnitOfWork.Dispose();
    }

    [TearDown]
    public void CleanUp()
    {
        UserManager.ClearSubstitute();
        SignInManager.ClearSubstitute();
        UnitOfWork.ClearSubstitute();
        Config.ClearSubstitute();
    }
}