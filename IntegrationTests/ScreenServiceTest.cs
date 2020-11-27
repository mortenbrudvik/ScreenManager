using System.Linq;
using System.Threading;
using Infrastructure;
using Infrastructure.Services;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class ScreenServiceTest
    {
        private readonly ITestOutputHelper _output;

        public ScreenServiceTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GetScreens_ShouldReturnAtLeastOne()
        {
            var service = new ScreenService();
            var screens = service.GetAll();

            screens.ToList().ForEach(screen => _output.WriteLine(screen.ToString()));
            
            screens.ShouldNotBeEmpty();
        }

        [Fact]
        public void GetScreens_OneScreenShouldBePrimary()
        {
            var service = new ScreenService();
            var screens = service.GetAll();
            
            screens.ShouldContain(screen => screen.IsPrimary, 1);
        }

        [Fact]
        public void Resolutions_ShouldReturnAtLeastOne()
        {
            var service = new ScreenService();
            var resolutions = service.GetAll()
                .Single(screen=>screen.IsPrimary)
                .GetResolutions().ToList();

            resolutions.ToList().ForEach(resolution => _output.WriteLine(resolution.ToString()));

            resolutions.ShouldNotBeEmpty();
        }

        [Fact]
        public void ScreensChanged_ShouldFire_WhenScreenResolutionChanges()
        {
            var service = new ScreenService();
            var hasChanged = false;

            service.Changed += (sender, args) => { hasChanged = true; };

            var primaryScreen = service.GetAll().Single(screen => screen.IsPrimary);
            var displayName = primaryScreen.Name;
            var oldResolution = primaryScreen.Resolution;
            var newResolution = primaryScreen.GetResolutions().OrderByDescending(screen => screen).Skip(1).FirstOrDefault();

            _output.WriteLine("Changing resolution to: " + newResolution);
            ScreenUtils.ChangeResolution(displayName, newResolution);

            Thread.Sleep(1000);

            hasChanged.ShouldBe(true);

            _output.WriteLine("Changing resolution back to: " + oldResolution);
            ScreenUtils.ChangeResolution(displayName, oldResolution);
        }
    }
}
