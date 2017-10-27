using System.Threading.Tasks;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.SkiaEngine;
using Xamarin.Forms;
using NavigationEventArgs = GemSwipe.Game.Navigation.NavigationEventArgs;
using Rectangle = GemSwipe.Game.Geometry.Rectangle;

namespace GemSwipe.Game.Layers
{
    public class LoadingLayer : SkiaView
    {
        private Rectangle _transitionScreen;

        public LoadingLayer()
        {
            _transitionScreen = new Rectangle(0, 0, Height, Width, CreateColor(250, 247, 240));

            AddChild(_transitionScreen);

            Navigator.NavigationStarted += Navigator_NavigationStarted;
            Navigator.InitialNavigationStarted += Navigator_InitialNavigationStarted;
        }

        private void Navigator_InitialNavigationStarted()
        {
            this.Animate("whiteSmokeOut", p => _transitionScreen.Opacity = (float)p, _transitionScreen.Opacity, 0, 8, (uint)2000, Easing.Linear);
        }

        private async void Navigator_NavigationStarted(NavigationEventArgs arg)
        {
            this.Animate("whiteSmokeIn", p => _transitionScreen.Opacity = (float)p, _transitionScreen.Opacity, 1, 8, (uint)1000, Easing.Linear);
            await Task.Delay(1500);
            this.Animate("whiteSmokeOut", p => _transitionScreen.Opacity = (float)p, _transitionScreen.Opacity, 0, 8, (uint)1000, Easing.Linear);
        }

        protected override void Draw()
        {

        }
    }
}
