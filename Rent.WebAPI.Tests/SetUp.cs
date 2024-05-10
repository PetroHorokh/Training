using NSubstitute;
using Rent.BLL.Services.Contracts;
using Rent.WebAPI.Controllers;

namespace Rent.WebAPI.Tests;

public class SetUp
{
    public static readonly ITenantService Service = Substitute.For<ITenantService>();
    public readonly TenantController Controller = new TenantController(Service);

    [OneTimeSetUp]
    public void Setup()
    {
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        Controller.Dispose();
    }
}