namespace Rent.console.Handles;

public class MenuHandle
{
    public delegate Task MenuDelegate();
    public static readonly List<MenuDelegate> MainMenuHandle;

    public static int MainMenuSelector { get; set; }
    public static int PrevSelector { get; set; }

    static MenuHandle()
    {
        MainMenuHandle = 
        [
            MainMenu,
            TenantMenu,
            OwnerMenu,
            RoomMenu,
            ViewMenu,
            AddressMenu,
            () => Task.Run(() =>
            {
                Program.Working = false;
                Console.WriteLine("Bye! Take care!");
            })
        ];
        MainMenuSelector = 0;
        PrevSelector = 0;
    }

    private static Task MainMenu()
    {
        PrevSelector = 0;
        Console.WriteLine("\nMain menu" +
                          "\n1.Tenant menu" +
                          "\n2.Owner menu" +
                          "\n3.Room menu" +
                          "\n4.View menu" +
                          "\n5.Address menu" +
                          "\n6.Exit");
        
        Console.Write("\nSelect an option: ");
        string input = Console.ReadLine()!;

        _ = int.TryParse(input, out int select);

        MainMenuSelector = select;
        return Task.CompletedTask;
    }

    private static async Task TenantMenu()
    {
        PrevSelector = 1;
        Console.WriteLine(
            "\nTenant menu" +
            "\n1.Get all tenants" +
            "\n2.Get all bills" +
            "\n3.Get tenant by name" +
            "\n4.Get tenant address information" +
            "\n5.Get tenant rents" +
            "\n6.Get tenant bills" +
            "\n7.Get available assets" +
            "\n8.Get booking for a asset" +
            "\n9.Create tenant" +
            "\n10.Create rent" +
            "\n11.Create payment" +
            "\n12.Update tenant" +
            "\n13.Cancel rent" +
            "\n14.Delete tenant" +
            "\n15.Exit");

        Console.Write("\nSelect an option: ");
        string input = Console.ReadLine()!;
        _ = int.TryParse(input, out int select);
        
        await TenantHandle.TenantMenu[select - 1]();
    }

    private static async Task OwnerMenu()
    {
        PrevSelector = 2;
        Console.WriteLine(
            "\nOwner menu" +
            "\n1.Get all owners" +
            "\n2.Get all assets" +
            "\n3.Get owner by id" +
            "\n4.Get owner address information" +
            "\n5.Get owner assets" +
            "\n6.Create owner" +
            "\n7.Delete owner" +
            "\n8.Create asset" +
            "\n9.Delete asset" +
            "\n10.Exit");

        Console.Write("\nSelect an option: ");
        string input = Console.ReadLine()!;
        _ = int.TryParse(input, out int select);

        await OwnerHandle.OwnerMenu[select - 1]();
    }

    private static async Task RoomMenu()
    {
        PrevSelector = 3;
        Console.WriteLine(
            "\nRoom menu" +
            "\n1.Get all rooms" +
            "\n2.Get all room types" +
            "\n3.Get all accommodations" +
            "\n4.Get room by room id" +
            "\n5.Get room by room number" +
            "\n6.Get room's accommodations" +
            "\n7.Create room" +
            "\n8.Create room type" +
            "\n9.Create accommodation" +
            "\n10.Create room accommodation" +
            "\n11.Update room accommodation" +
            "\n12.Delete room" +
            "\n13.Delete room accommodation" +
            "\n14.Exit");

        Console.Write("\nSelect an option: ");
        string input = Console.ReadLine()!;
        _ = int.TryParse(input, out int select);

        await RoomHandle.RoomMenu[select - 1]();
    }

    private static async Task ViewMenu()
    {
        PrevSelector = 4;
        Console.WriteLine(
            "\nView menu" +
            "\n1.Certificate for tenant" +
            "\n2.See room occupation in give date" +
            "\n3.Get general information for tenant" +
            "\n4.Exit");

        Console.Write("\nSelect an option: ");
        string input = Console.ReadLine()!;
        _ = int.TryParse(input, out int select);

        await ViewHandle.ViewMenu[select - 1]();
    }

    private static async Task AddressMenu()
    {
        PrevSelector = 5;
        Console.WriteLine(
            "\nAddress menu" +
            "\n1.Get all addresses" +
            "\n2.Get address by id" +
            "\n3.Exit");

        Console.Write("\nSelect an option: ");
        string input = Console.ReadLine()!;
        _ = int.TryParse(input, out int select);

        await AddressHandle.AddressMenu[select - 1]();
    }
}