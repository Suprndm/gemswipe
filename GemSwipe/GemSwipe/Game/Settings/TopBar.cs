using System;
using System.Threading.Tasks;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities;
using GemSwipe.Utilities.Buttons;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings
{
    public class TopBar:SkiaView
    {
        public event Action SettingsButtonPressed;

        public TopBar(float x, float y, float height, float width) : base(x, y, height, width)
        {
            _height = 0.1f * height;

            var settingsButton = new SimpleButton(width - _height / 2, _height / 2, _height, _height);
            AddChild(settingsButton);
            DeclareTappable(settingsButton);
            _y = -height;
            settingsButton.Activated += SettingsButton_Tapped;

            DeclareTappable(this);
        }

        public Task Show()
        {
            this.Animate("slideIn", p => _y = (float)p, -Height, 0f, 8, (uint)300, Easing.CubicOut);
            return Task.Delay(300);
        }

        public Task Hide()
        {
            this.Animate("slideOut", p => _y = (float)p, _y, -Height, 8, (uint)300, Easing.CubicOut);
            return Task.Delay(300);
        }

        private void SettingsButton_Tapped()
        {
            SettingsButtonPressed?.Invoke();
        }

        protected override void Draw()
        {
            DrawHitbox();

        }
    }
}
