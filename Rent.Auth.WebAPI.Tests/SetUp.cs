using NSubstitute;
using NSubstitute.ClearExtensions;
using Rent.Auth.BLL.Services.Contracts;
using Rent.Auth.WebAPI.Controllers;

namespace Rent.Auth.WebAPI.Tests;

public class SetUp
{
    public static readonly IUserService Service = Substitute.For<IUserService>();
    public readonly AuthController Controller = new(Service);

    [OneTimeSetUp]
    public void GlobalSetup()
    {
    }

    [OneTimeTearDown]
    public void GlobalCleanUp()
    {
        Controller.Dispose();
    }

    [TearDown]
    public void CleanUp()
    {
        Service.ClearSubstitute();
    }
}