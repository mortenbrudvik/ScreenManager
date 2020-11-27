using System.Linq;
using System.Threading;
using Infrastructure.Services;
using static System.Console;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ScreenManager();
            manager.Changed += (sender, eventArgs) => WriteLine("Display settings changed");

            // Get all screens
            var screens = manager.GetAll();
            screens.ToList().ForEach(WriteLine);
            WriteLine();

            // Get all resolutions for primary screen
            var primary = manager.GetPrimary();
            var resolutions = primary.GetResolutions();
            resolutions.ToList().ForEach(r => WriteLine(r));
            WriteLine();

            // Change resolution for primary screen
            var newResolution = resolutions.OrderByDescending(s => s).Skip(1).FirstOrDefault();
            var previousResolution = primary.Resolution;
            WriteLine("Change resolution to " + newResolution);

            primary.ChangeResolution(newResolution);

            Thread.Sleep(2000);

            // Change back to previous resolution
            WriteLine("Change resolution back to " + previousResolution);
            primary.ChangeResolution(previousResolution);
            
            ReadLine();
        }
    }
}
