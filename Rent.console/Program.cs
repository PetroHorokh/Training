using Microsoft.Extensions.DependencyInjection;
using Rent.ADO.NET;
using Rent.BLL;
using Rent.console.Handles;

namespace Rent.console;

internal class Program
{
    public static bool Working = true;

    public static ServiceProvider BllServices = BllServiceProvider.ServiceConfiguration();
    public static ServiceProvider AdoNetServiced = AdoNetServiceProvider.ServiceConfiguration();

    private static async Task Main()
    {
        do
        {
            try
            {
                await MenuHandle.MainMenuHandle[MenuHandle.MainMenuSelector]();
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong! Try again");
                MenuHandle.MainMenuSelector = MenuHandle.PrevSelector;
            }
        } while (Working);
    }
}