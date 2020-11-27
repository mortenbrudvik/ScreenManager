using System.Linq;
using System.Threading;
using System.Windows;
using Infrastructure;
using Infrastructure.Services;
using Shouldly;
using Xunit;

namespace IntegrationTests
{
    public class ScreenServiceTest
    {
        [Fact]
        public void GetScreens_ShouldReturnAtLeastOne()
        {
            var service = new ScreenService();
            var screens = service.GetAll();
            
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
                .Resolutions.ToList();

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
            var newResolution = primaryScreen.Resolutions.OrderByDescending(screen => screen).Skip(1).FirstOrDefault();

            ScreenUtils.ChangeResolution(displayName, out var oldResolution, new Size(newResolution.Width, newResolution.Height));

            Thread.Sleep(1000);

            hasChanged.ShouldBe(hasChanged);

            ScreenUtils.ChangeResolution(displayName, out var _, new Size(oldResolution.Width, oldResolution.Height));
        }
    }
}
