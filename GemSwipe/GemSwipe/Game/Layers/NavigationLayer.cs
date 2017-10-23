using GemSwipe.Game.Navigation;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Pages.Home;
using GemSwipe.Game.Pages.Map;
using GemSwipe.Game.SkiaEngine;

namespace GemSwipe.Game.Layers
{
    public class NavigationLayer:SkiaView
    {
        public NavigationLayer(float height, float width) : base(0, 0, height, width)
        {
            SetupNavigation();
        }

        public void SetupNavigation()
        {
            var homePage = new HomePage(0, 0, Height, Width);
            AddChild(homePage);
            var mapPage = new MapPage(0, 0, Height, Width);
            AddChild(mapPage);
            var gamePage = new GamePage(0, 0, Height, Width);
            AddChild(gamePage);

            Navigator.Instance.RegisterPage(PageType.Home, homePage);
            Navigator.Instance.RegisterPage(PageType.Map, mapPage);
            Navigator.Instance.RegisterPage(PageType.Game, gamePage);
            Navigator.Instance.GoToInitialPage(PageType.Home);
        }

        protected override void Draw()
        {
            
        }
    }
}
