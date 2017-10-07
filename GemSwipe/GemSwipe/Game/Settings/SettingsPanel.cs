using System.Threading.Tasks;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Navigation.Pages;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings
{
    public class SettingsPanel : PageBase
    {
        public SettingsPanel(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _x = width;
            DeclareTappable(this);
            Tapped += SettingsPanel_Tapped;
            _opacity = 1;
        }

        private void SettingsPanel_Tapped()
        {
            Navigator.Instance.HideSettings();
        }

        protected override void OnActivated(object parameter = null)
        {
        }

        protected override void OnDeactivated()
        {
        }

        protected override Task TransitionIn()
        {
            this.Animate("slideOut", p => _x = (float)p, _x, 0f, 8, (uint)300, Easing.SpringOut);
            return Task.Delay(300);
        }

        protected override Task TransitionOut()
        {
            this.Animate("slideOut", p => _x = (float)p, _x, Width, 8, (uint)300, Easing.SpringIn);
            return Task.Delay(300);
        }


        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                var panelHeight = Height * 0.9f;
                var panelWidth = Width * 0.8f;
                paint.IsAntialias = true;
                paint.Color = CreateColor(168, 174, 240);

                Canvas.DrawRect(SKRect.Create(X + (Width - panelWidth), Y+(Height- panelHeight) , panelWidth*1.2f, panelHeight), paint);
            }
        }
    }
}
