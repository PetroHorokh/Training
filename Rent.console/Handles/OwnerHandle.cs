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
        var response = await OwnerService.GetAllOwnersAsync();

        if (!response.Exceptions.IsNullOrEmpty())
        {
            Console.WriteLine($"Error occured");
            return;
        }

        if (response.Body.IsNullOrEmpty()) Console.WriteLine("There are no owners");
        else
        {
            foreach (var owner in response.Body!)
            {
                Console.WriteLine(owner);
            }
        }
    }

    private static async Task GetAllAssetsAsync()
    {
        var response = await OwnerService.GetAllAssetsAsync();

        if (!response.Exceptions.IsNullOrEmpty())
        {
            Console.WriteLine($"Error occured");
            return;
        }

        if (response.Body.IsNullOrEmpty()) Console.WriteLine("There are no assets");
        else
        {
            foreach (var asset in response.Body!)
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
            var response = await OwnerService.GetOwnerByIdAsync(ownerId);

            if (!response.Exceptions.IsNullOrEmpty())
            {
                Console.WriteLine($"Error occured");
                return;
            }

            Console.WriteLine(response.Body is not null ? response : "Error occured");
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
            var response = await OwnerService.GetOwnerAssetsAsync(ownerId);

            if (!response.Exceptions.IsNullOrEmpty())
            {
                Console.WriteLine($"Error occured");
                return;
            }

            if (response.Body.IsNullOrEmpty()) Console.WriteLine("\nThere are no assets");
            else
            {
                foreach (var asset in response.Body!)
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

        Console.WriteLine(!result.Exceptions.IsNullOrEmpty()
            ? "\nError occured"
            : $"\nSuccessfully created a new owner with id {result.Body}");
    }

    private static async Task DeleteOwnerAsync()
    {
        Console.Write("\nPlease enter required owner id for deletion: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out var ownerId))
        {
            var result = await OwnerService.DeleteOwnerAsync(ownerId);

            Console.WriteLine(
                !result.Exceptions.IsNullOrEmpty() ? "\nError occured" : "\nSuccessfully deleted an owner");
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

        Console.WriteLine(!result.Exceptions.IsNullOrEmpty()
            ? "\nError occured"
            : $"\nSuccessfully created a new asset with id {result.Body}");
    }

    private static async Task DeleteAssetAsync()
    {
        Console.Write("\nPlease enter required asset id for deletion: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out var assetId))
        {
            var result = await OwnerService.DeleteAssetAsync(assetId);

            Console.WriteLine(
                !result.Exceptions.IsNullOrEmpty() ? "\nError occured" : "\nSuccessfully deleted an asset");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }
}