using Microsoft.Extensions.DependencyInjection;
using Rent.ADO.NET;
using Rent.BLL;
using Rent.console.Handles;

namespace Rent.console;

internal class Program
{
    public static bool Working = true;

    public static IServiceProvider Services { get; private set; }

    static Program()
    {
        Services = new ServiceCollection().BllServiceInject().AdoNetServiceInject().BuildServiceProvider();
    }

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