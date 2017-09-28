using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.Menu;
using GemSwipe.GameEngine.Navigation;
using GemSwipe.GameEngine.Navigation.Pages;
using SkiaSharp;

namespace GemSwipe.GameEngine.Map
{
    public class MapPage:PageBase
    {
        public MapPage(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {

            AddChild(new TextBlock(canvas, width / 2, height / 2, "This is the map !", height / 30f,
                new SKColor(255, 255, 255)));


            AddChild(new TextBlock(canvas, width / 2, 2*height / 3, "Again, tap to play...", height / 50f,
                new SKColor(255, 255, 255)));

            DeclareTappable(this);
            Tapped += TappedHandler;
        }

        private void TappedHandler()
        {
            Navigator.Instance.GoTo(PageType.Game);
        }

        protected override void Draw()
        {
        }

        protected override void OnActivated()
        {
        }

        protected override void OnDeactivated()
        {
        }
    }
}
