using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.Menu;
using GemSwipe.GameEngine.Navigation;
using GemSwipe.GameEngine.Navigation.Pages;
using SkiaSharp;

namespace GemSwipe.GameEngine.Home
{
    public class HomePage: PageBase
    {
        public HomePage(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            var tapToPlay = new TapToPlay(canvas, 0, 0, height, width);
            AddChild(tapToPlay);
            tapToPlay.Tapped += OnTapped;
        }

        private void OnTapped()
        {
            Navigator.Instance.GoTo(PageType.Map);
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
