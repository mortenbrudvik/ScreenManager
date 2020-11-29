using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ApplicationCore.Interfaces;
using Infrastructure;
using Infrastructure.Services;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class ScreenManagerTest
    {
        private readonly ITestOutputHelper _output;

        public ScreenManagerTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GetScreens_ShouldReturnAtLeastOne()
        {
            var manager = new ScreenManager();
            var screens = manager.GetAll();

            screens.ToList().ForEach(screen => _output.WriteLine(screen.ToString()));
            
            screens.ShouldNotBeEmpty();
        }

        [Fact]
        public void GetScreens_OneScreenShouldBePrimary()
        {
            var manager = new ScreenManager();
            var screens = manager.GetAll();
            
            screens.ShouldContain(screen => screen.IsPrimary, 1);
        }

        [Fact]
        public void GetPrimary_ShouldReturnPrimaryScreen()
        {
            var manager = new ScreenManager();
            var primary = manager.GetPrimary();

            primary.IsPrimary.ShouldBeTrue();
        }

        [Fact]
        public void Resolutions_ShouldReturnAtLeastOne()
        {
            var manager = new ScreenManager();
            var resolutions = manager.GetAll()
                .Single(screen=>screen.IsPrimary)
                .GetResolutions().ToList();

            resolutions.ToList().ForEach(resolution => _output.WriteLine(resolution.ToString()));

            resolutions.ShouldNotBeEmpty();
        }

        [Fact]
        public void ScreensChanged_ShouldFireAndResolutionShowMatchChange_WhenScreenResolutionChanges()
        {
            var manager = new ScreenManager();
            var hasChanged = false;
            IReadOnlyCollection<IScreen> updatedScreens = null;

            manager.Changed += (sender, args) =>
            {
                hasChanged = true;
                updatedScreens = args.Screens;
            };

            var primaryScreen = manager.GetAll().Single(screen => screen.IsPrimary);
            var displayName = primaryScreen.Name;
            var oldResolution = primaryScreen.Resolution;
            var newResolution = primaryScreen.GetResolutions().OrderByDescending(screen => screen).Skip(1).FirstOrDefault();

            _output.WriteLine("Changing resolution to: " + newResolution);
            ScreenUtils.ChangeResolution(displayName, newResolution);

            Thread.Sleep(1000);

            hasChanged.ShouldBeTrue();
            updatedScreens.ShouldNotBeNull();
            updatedScreens.ShouldContain(screen => screen.IsPrimary && screen.Resolution == newResolution, 1);

            _output.WriteLine("Changing resolution back to: " + oldResolution);
            ScreenUtils.ChangeResolution(displayName, oldResolution);
        }
    }
}
