using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Navigation.Pages;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Pages.Home
{
    public class HomePage: PageBase
    {
        public HomePage( float x, float y, float height, float width) : base( x, y, height, width)
        {
            var tapToPlay = new TapToPlay( 0, 0, height, width);
            AddChild(tapToPlay);
            tapToPlay.Down += OnTapped;
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
