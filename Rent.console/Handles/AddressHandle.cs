using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rent.ADO.NET.Services.Contracts;
using Rent.BLL.Services;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;

namespace Rent.console.Handles;

public static class AddressHandle
{
    public delegate Task AddressHandleDelegate();
    public static List<AddressHandleDelegate> AddressMenu { get; set; }

    private static readonly IConnectedArchitecture ConnectedArchitectureService;

    static AddressHandle()
    {
        ConnectedArchitectureService = Program.Services.GetRequiredService<IConnectedArchitecture>();
        AddressMenu =
        [
            GetAllOwnersAsync,
            GetAddressByIdAsync,
            () => Task.Run(() => MenuHandle.MainMenuSelector = 0),
        ];
    }

    private static async Task GetAllOwnersAsync()
    {
        var addresses = (await ConnectedArchitectureService.GetAllAddressesAsync()).ToList();

        if (addresses.IsNullOrEmpty()) Console.WriteLine("There are no addresses");
        else
        {
            foreach (var address in addresses)
            {
                Console.WriteLine(address);
            }
        }
    }

    private static async Task GetAddressByIdAsync()
    {
        Console.Write("\nPlease enter address id: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid addressId))
        {
            var address = await ConnectedArchitectureService.GetAddressByIdAsync(addressId);

            Console.WriteLine(address != null ?
                address :
                "\nThere is no such address");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }
}