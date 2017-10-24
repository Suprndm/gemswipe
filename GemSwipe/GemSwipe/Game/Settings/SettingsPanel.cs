using System.Threading.Tasks;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Navigation.Pages;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings
{
    public class SettingsPanel : SkiaView
    {

        public bool IsShowed { get; set; }
        
        public SettingsPanel(float x, float y, float height, float width) : base(x, y, height, width)
        {
            _x = width;
            DeclareTappable(this);
            Down += SettingsPanel_Tapped;
            _opacity = 1;
        }

        private void SettingsPanel_Tapped()
        {
            Hide();
        }

        public Task Show()
        {
            IsShowed = true;
            this.Animate("slideIn", p => _x = (float)p, _x, 0f, 8, (uint)300, Easing.SpringOut);
            return Task.Delay(300);
        }

        public Task Hide()
        {
            IsShowed = false;
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

                Canvas.DrawRect(SKRect.Create(X + (Width - panelWidth), Y + (Height - panelHeight), panelWidth * 1.2f, panelHeight), paint);
            }
        }
    }
}
