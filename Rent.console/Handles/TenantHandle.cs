using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rent.ADO.NET.Services;
using Rent.ADO.NET.Services.Contracts;
using Rent.BLL.Services;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;

namespace Rent.console.Handles;

public static class TenantHandle
{
    public delegate Task TenantHandleDelegate();
    public static List<TenantHandleDelegate> TenantMenu { get; set; }

    private static readonly ITenantService TenantService;
    private static readonly IDisconnectedArchitecture DisconnectedArchitectureService;

    static TenantHandle()
    {
        TenantService = Program.Services.GetRequiredService<ITenantService>();
        DisconnectedArchitectureService = Program.Services.GetRequiredService<IDisconnectedArchitecture>();
        TenantMenu =
        [ 
            GetAllTenantsAsync,
            GetAllBillsAsync,
            GetTenantRentsAsync,
            GetTenantBillsAsync,
            GetAvailableAssets,
            GetAssetBookingAsync,
            CreateTenantAsync,
            CreateRentAsync,
            CreatePaymentAsync,
            UpdateTenantAsync,
            CancelRentAsync,
            DeleteTenantAsync,
            () => Task.Run(() => MenuHandle.MainMenuSelector = 0),
        ];
    }

    private static async Task GetAllTenantsAsync()
    {
        var response = await TenantService.GetAllTenantsAsync();

        if (response.Collection.IsNullOrEmpty()) Console.WriteLine("There are no tenants");
        else
        {
            foreach (var tenant in response.Collection!)
            {
                Console.WriteLine(tenant);
            }
        }
    }

    private static async Task GetAllBillsAsync()
    {
        var response = await TenantService.GetAllBillsAsync();

        if (response.Collection.IsNullOrEmpty()) Console.WriteLine("There are no bills");
        else
        {
            foreach (var bill in response.Collection!)
            {
                Console.WriteLine(bill);
            }
        }
    }

    private static async Task GetTenantRentsAsync()
    {
        Console.WriteLine("\nPlease enter required tenant id: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid tenantId))
        {
            var rents = await TenantService.GetTenantRentsAsync(tenantId);

            if (rents.Collection.IsNullOrEmpty()) Console.WriteLine("There are no rents");
            else
            {
                foreach (var rent in rents.Collection!)
                {
                    Console.WriteLine(rent);
                }
            }
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task GetTenantBillsAsync()
    {
        Console.WriteLine("\nPlease enter required tenant id: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid tenantId))
        {
            var response = await TenantService.GetTenantBillsAsync(tenantId);

            if (response.Collection.IsNullOrEmpty()) Console.WriteLine("There are no bills");
            else
            {
                foreach (var bill in response.Collection!)
                {
                    Console.WriteLine(bill);
                }
            }
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task GetAvailableAssets()
    {
        Console.Write("\nPlease enter desirable date: ");
        string input = Console.ReadLine()!;

        if (DateTime.TryParse(input, out DateTime dateTime))
        {
            var assets = (await DisconnectedArchitectureService.GetAvailableAssetsAsync(dateTime)).ToList();

            if (assets.IsNullOrEmpty()) Console.WriteLine("\nThere are no available assets");
            else
            {
                Console.WriteLine("\nAvailable asset ids: ");
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

    private static async Task GetAssetBookingAsync()
    {
        Console.Write("\nPlease enter asset id: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid assetId))
        {
            var rents = (await DisconnectedArchitectureService.GetAssetBookingAsync(assetId)).ToList();

            if (rents.IsNullOrEmpty()) Console.WriteLine("\nThere are no booking for this asset");
            else
            {
                foreach (var rent in rents)
                {
                    Console.WriteLine(rent);
                }
            }
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task CreateTenantAsync()
    {
        Console.WriteLine("\nCreation of new tenant");

        string name, bankName, description, director, input;
        Guid addressId;

        do
        {
            Console.Write("Enter name: ");
            name = Console.ReadLine()!;
        } while (name.Length == 0);

        do
        {
            Console.Write("Enter bank name: ");
            bankName = Console.ReadLine()!;
        } while (bankName.Length == 0);

        do
        {
            Console.Write("Enter address id: ");
            input = Console.ReadLine()!;
        } while (!Guid.TryParse(input, out addressId));

        do
        {
            Console.Write("Enter description: ");
            description = Console.ReadLine()!;
        } while (description.Length == 0);

        do
        {
            Console.Write("Enter director: ");
            director = Console.ReadLine()!;
        } while (director.Length == 0);

        var tenant = new TenantToCreateDto()
        {
            Name = name,
            BankName = bankName,
            Description = description,
            Director = director,
            AddressId = addressId
        };

        var result = await TenantService.CreateTenantAsync(tenant);

        Console.WriteLine(result.Error != null ? '\n' + result.Error.Message : $"\nSuccessfully created a new tenant with id {result.CreatedId}");
    }

    private static async Task CreateRentAsync()
    {
        Console.WriteLine("\nCreation of new Rent");

        DateTime startDate, endDate;
        Guid tenantId, assetId;
        string input;

        do
        {
            Console.Write("Enter tenant id: ");
            input = Console.ReadLine()!;
        } while (!Guid.TryParse(input, out tenantId));

        do
        {
            Console.Write("Enter asset id: ");
            input = Console.ReadLine()!;
        } while (!Guid.TryParse(input, out assetId));

        do
        {
            do
            {
                Console.Write("Enter start date: ");
                input = Console.ReadLine()!;
            } while (!DateTime.TryParse(input, out startDate));

            do
            {
                Console.Write("Enter end date: ");
                input = Console.ReadLine()!;
            } while (!DateTime.TryParse(input, out endDate));
        } while (endDate <= startDate);

        var rent = new RentToCreateDto()
        {
            AssetId = assetId,
            TenantId = tenantId,
            StartDate = startDate,
            EndDate = endDate
        };

        var result = await TenantService.CreateRentAsync(rent);

        Console.WriteLine(result.Error != null ? '\n' + result.Error.Message : $"\nSuccessfully created a new rent with id {result.CreatedId}");
    }

    private static async Task CreatePaymentAsync()
    {
        Console.WriteLine("\nCreation of new payment");

        Guid tenantId, billId;
        decimal amount;
        string input;

        do
        {
            Console.Write("Enter tenant id: ");
            input = Console.ReadLine()!;
        } while (!Guid.TryParse(input, out tenantId));

        do
        {
            Console.Write("Enter bill id: ");
            input = Console.ReadLine()!;
        } while (!Guid.TryParse(input, out billId));

        do
        {
            Console.Write("Enter amount to pay: ");
            input = Console.ReadLine()!;
        } while (!decimal.TryParse(input, out amount));

        var payment = new PaymentToCreateDto()
        {
            TenantId = tenantId,
            BillId = billId,
            Amount = amount
        };

        var result = await TenantService.CreatePaymentAsync(payment);

        Console.WriteLine(result.Error != null ? '\n' + result.Error.Message : $"\nSuccessfully created a new payment with id {result.CreatedId}");
    }

    private static async Task UpdateTenantAsync()
    {
        Console.Write("\nPlease enter required tenant id for update: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out var tenantId))
        {
            var tenant = await TenantService.GetTenantByIdAsync(tenantId);

            if (tenant.Entity is not null)
            {
                Console.WriteLine("Tenant edit");

                var tenantName = tenant.Entity.Name;
                EditProp(ref tenantName, "name");
                tenant.Entity.Name = tenantName;

                var tenantBankName = tenant.Entity.BankName;
                EditProp(ref tenantBankName, "bank");
                tenant.Entity.BankName = tenantBankName;

                var tenantDirector = tenant.Entity.Director;
                EditProp(ref tenantDirector, "director");
                tenant.Entity.Director = tenantDirector;

                var tenantDescription = tenant.Entity.Description;
                EditProp(ref tenantDescription, "description");
                tenant.Entity.Description = tenantDescription;

                var tenantAddressId = tenant.Entity.AddressId;
                EditGuidProp(ref tenantAddressId, "address id");
                tenant.Entity.AddressId = tenantAddressId;

                var tenantToUpdate = new TenantToGetDto()
                {
                    Name = tenant.Entity.Name,
                    BankName = tenant.Entity.BankName,
                    Director = tenant.Entity.Director,
                    Description = tenant.Entity.Description,
                    AddressId = tenantAddressId
                };

                var result = await TenantService.UpdateTenantAsync(tenantToUpdate);

                Console.WriteLine(result.Error == null
                    ? $"Changes were saved"
                    : $"Changes were not saved due to an error with message: {result.Error.InnerException}");
            }
            else
            {
                Console.WriteLine("\nThere is no such tenant");
            }
        }
        else
        {
            Console.WriteLine("\nWrong name format");
        }
    }

    private static async Task CancelRentAsync()
    {
        Console.Write("\nPlease enter rent id for cancellation: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid rentId))
        {
            var result = await TenantService.CancelRentAsync(rentId);

            Console.WriteLine(result.Error == null
                ? $"Cancellation was a success"
                : $"Cancellation was not able to be saved due to an error with message: {result.Error.InnerException}");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task DeleteTenantAsync()
    {
        Console.Write("\nPlease enter required tenant id for deletion: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid tenantId))
        {
            var result = await TenantService.DeleteTenantAsync(tenantId);

            Console.WriteLine(result.Error != null ? '\n' + result.Error.Message : "\nSuccessfully deleted a tenant");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static bool EditDecision(string input) =>
        string.CompareOrdinal(input.ToLower(), "y") == 0 || string.CompareOrdinal(input.ToLower(), "n") == 0 ||
        string.CompareOrdinal(input.ToLower(), "yes") == 0 || string.CompareOrdinal(input.ToLower(), "no") == 0;

    private static bool EditConfirm(string input) => string.CompareOrdinal(input.ToLower(), "y") == 0 ||
                                                     string.CompareOrdinal(input.ToLower(), "yes") == 0;

    private static void EditProp(ref string prop, string propName)
    {
        string input;
        Console.WriteLine($"Old tenant {propName}: {prop}");

        do
        {
            Console.Write($"Would you like to change tenant's {propName}? (Y/N): ");
            input = Console.ReadLine()!;
        } while (!EditDecision(input));

        if (EditConfirm(input))
        {
            Console.Write($"New tenant {propName}: ");
            input = Console.ReadLine()!;
            prop = input;
        }
    }

    private static void EditGuidProp(ref Guid prop, string propName)
    {
        string input;
        Console.WriteLine($"Old tenant {propName}: {prop}");

        do
        {
            Console.Write($"Would you like to change tenant's {propName}? (Y/N): ");
            input = Console.ReadLine()!;
        } while (!EditDecision(input));

        if (EditConfirm(input))
        {
            Guid newProp;
            do
            {
                Console.Write($"New tenant {propName}: ");
                input = Console.ReadLine()!;
            } while (!Guid.TryParse(input, out newProp));
            prop = newProp;
        }
    }
}