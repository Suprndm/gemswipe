using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Pages.Home;
using GemSwipe.Game.Pages.Map;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Entities
{
    public class SkiaRoot : SkiaView
    {
        
        private readonly Background _background;

        public SkiaRoot(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            // RegisterPages 
            var homePage = new HomePage(canvas, 0, 0, height, width);
            AddChild(homePage);
            var mapPage = new MapPage(canvas, 0, 0, height, width);
            AddChild(mapPage);
            var gamePage = new GamePage(canvas, 0, 0, height, width);
            AddChild(gamePage);
            Navigator.Instance.RegisterPage(PageType.Home, homePage);
            Navigator.Instance.RegisterPage(PageType.Map, mapPage);
            Navigator.Instance.RegisterPage(PageType.Game, gamePage);
            Navigator.Instance.GoToInitialPage(PageType.Home);


            _background = new Background(canvas, 0, 0, height, width);
            AddChild(_background, -1);
        }

        protected override void Draw()
        {

        }


        public override void Dispose()
        {
        }
    }
}