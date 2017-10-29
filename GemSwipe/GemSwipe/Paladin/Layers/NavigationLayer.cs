using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Pages.Home;
using GemSwipe.Game.Pages.Map;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Navigation;

namespace GemSwipe.Paladin.Layers
{
    public class NavigationLayer:SkiaView
    {
        public NavigationLayer()
        {
         
            SetupNavigation();
        }

        public void SetupNavigation()
        {
            var homePage = new HomePage();
            AddChild(homePage);
            var mapPage = new MapPage();
            AddChild(mapPage);
            var gamePage = new GamePage();
            AddChild(gamePage);

            Navigator.Instance.RegisterPage(PageType.Home, homePage);
            Navigator.Instance.RegisterPage(PageType.Map, mapPage);
            Navigator.Instance.RegisterPage(PageType.Game, gamePage);
        }

   

        protected override void Draw()
        {
            
        }
    }
}
