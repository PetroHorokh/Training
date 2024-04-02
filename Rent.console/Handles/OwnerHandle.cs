using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rent.BLL.Services;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;

namespace Rent.console.Handles;

public class OwnerHandle
{
    public delegate Task OwnerHandleDelegate();
    public static List<OwnerHandleDelegate> OwnerMenu { get; set; }

    private static readonly IOwnerService OwnerService;

    static OwnerHandle()
    {
        OwnerService = Program.Services.GetRequiredService<IOwnerService>();
        OwnerMenu =
        [
            GetAllOwnersAsync,
            GetAllAssetsAsync,
            GetOwnerByIdAsync,
            GetOwnerAddressAsync,
            GetOwnerAssetsAsync,
            CreateOwnerAsync,
            DeleteOwnerAsync,
            CreateAssetAsync,
            DeleteAssetAsync,
            () => Task.Run(() => MenuHandle.MainMenuSelector = 0),
        ];
    }

    private static async Task GetAllOwnersAsync()
    {
        var owners = (await OwnerService.GetAllOwnersAsync()).ToList();

        if (owners.IsNullOrEmpty()) Console.WriteLine("There are no owners");
        else
        {
            foreach (var owner in owners)
            {
                Console.WriteLine(owner);
            }
        }
    }

    private static async Task GetAllAssetsAsync()
    {
        var assets = (await OwnerService.GetAllAssetsAsync()).ToList();

        if (assets.IsNullOrEmpty()) Console.WriteLine("There are no assets");
        else
        {
            foreach (var asset in assets)
            {
                Console.WriteLine(asset);
            }
        }
    }

    private static async Task GetOwnerByIdAsync()
    {
        Console.Write("\nPlease enter required owner id: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid ownerId))
        {
            var owner = await OwnerService.GetOwnerByIdAsync(ownerId);

            Console.WriteLine(owner != null ?
                owner :
                "\nThere is no such owner");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task GetOwnerAddressAsync()
    {
        Console.Write("\nPlease enter required owner id: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid ownerId))
        {
            var address = await OwnerService.GetOwnerAddressAsync(ownerId);

            Console.WriteLine(address != null ?
                address :
                "\nThere is no such owner");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task GetOwnerAssetsAsync()
    {
        Console.Write("\nPlease enter required owner id: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid ownerId))
        {
            var assets = (await OwnerService.GetOwnerAssetsAsync(ownerId)).ToList();

            if (assets.IsNullOrEmpty()) Console.WriteLine("\nThere are no assets");
            else
            {
                foreach (var asset in assets)
                {
                    Console.WriteLine(asset);
                }
            }
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task CreateOwnerAsync()
    {
        Console.WriteLine("\nCreation of new owner");

        string name, input;
        Guid addressId;

        do
        {
            Console.Write("Enter owner's name: ");
            name = Console.ReadLine()!;
        } while (name.Length == 0);

        do
        {
            Console.Write("Enter address id: ");
            input = Console.ReadLine()!;
        } while (!Guid.TryParse(input, out addressId));

        var owner = new OwnerToCreateDto()
        {
            Name = name,
            AddressId = addressId
        };

        var result = await OwnerService.CreateOwnerAsync(owner);

        Console.WriteLine(result.Error != null ? '\n' + result.Error.Message : $"\nSuccessfully created a new owner with id {result.CreatedId}");
    }

    private static async Task DeleteOwnerAsync()
    {
        Console.Write("\nPlease enter required owner id for deletion: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out var ownerId))
        {
            var result = await OwnerService.DeleteOwnerAsync(ownerId);

            Console.WriteLine(result.Error != null ? '\n' + result.Error.Message : "\nSuccessfully deleted an owner");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task CreateAssetAsync()
    {
        Console.WriteLine("\nCreation of new asset");

        string input;
        Guid ownerId, roomId;

        do
        {
            Console.Write("Enter owner id: ");
            input = Console.ReadLine()!;
        } while (!Guid.TryParse(input, out ownerId));

        do
        {
            Console.Write("Enter room id: ");
            input = Console.ReadLine()!;
        } while (!Guid.TryParse(input, out roomId));

        var asset = new AssetToCreateDto()
        {
            OwnerId = ownerId,
            RoomId = roomId
        };

        var result = await OwnerService.CreateAssetAsync(asset);

        Console.WriteLine(result.Error != null ? '\n' + result.Error.Message : $"\nSuccessfully created a new asset with id {result.CreatedId}");
    }

    private static async Task DeleteAssetAsync()
    {
        Console.Write("\nPlease enter required asset id for deletion: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out var assetId))
        {
            var result = await OwnerService.DeleteAssetAsync(assetId);

            Console.WriteLine(result.Error != null ? '\n' + result.Error.Message : "\nSuccessfully deleted an asset");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }
}