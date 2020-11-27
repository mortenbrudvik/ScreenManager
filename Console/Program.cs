using System.Linq;
using Infrastructure.Services;
using static System.Console;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Hello World!");

            var service = new ScreenService();
            service.Changed += (sender, eventArgs) => WriteLine("Display settings changes");

            service.GetAll().ToList().ForEach(WriteLine);

            ReadLine();
        }
    }
}
