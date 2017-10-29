using System.Threading.Tasks;
using GemSwipe.Data.PlayerLife;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.UIElements.Buttons;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings
{
    public class SettingsPanel : SkiaView
    {

        public bool IsShowed { get; set; }
        
        public SettingsPanel()
        {
            _x = Width;
            DeclareTappable(this);
            Down += SettingsPanel_Tapped;
            _opacity = 1;

            var recoverButton = new TextButton(Width / 2, Height / 6, Height/40, "Recover Life");
            var backButton = new TextButton(Width / 2, Height / 4, Height/40, "Exit");

            recoverButton.Activated += () =>
            {
                PlayerLifeService.Instance.GainLife();
            };

            backButton.Activated += () =>
            {
                Navigator.Instance.GoTo(PageType.Map);
                Hide();
            };

            AddChild(recoverButton);
            AddChild(backButton);
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

                Canvas.DrawRect(SKRect.Create(X + (Width - panelWidth), Y, panelWidth * 1.2f, panelHeight), paint);
            }
        }
    }
}
