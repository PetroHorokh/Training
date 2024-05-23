using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Rent.Auth.BLL.Services.Contracts;
using Rent.Auth.WebAPI.Controllers;
using Rent.AWS.S3.Services.Contracts;

namespace Rent.Auth.WebAPI.Tests;

public class SetUp
{
    public static readonly IUserService UserService = Substitute.For<IUserService>();
    public static readonly IS3Service S3Service = Substitute.For<IS3Service>();
    public required AuthController Controller = new(UserService, S3Service);
    public required IFormFile File;

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

        Controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claims)
            }
        };

        var content = String.Empty;
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        stream.Position = 0;

        File = new FormFile(stream, 0, stream.Length, String.Empty, fileName);
    }

    [OneTimeTearDown]
    public void GlobalCleanUp()
    {
        Controller.Dispose();
    }

    [TearDown]
    public void CleanUp()
    {
        UserService.ClearSubstitute();
        //S3Service.ClearSubstitute();
    }
}