using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Navigation.Pages;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Home
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

        protected override void OnActivated(object parameter = null)
        {

        }

        protected override void OnDeactivated()
        {
        }
    }
}
