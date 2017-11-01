using System;
using System.Threading.Tasks;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.UIElements.Buttons;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings
{
    public class TopBar:SkiaView
    {
        public event Action SettingsButtonPressed;

        public TopBar()
        {
            _height = 0.2f * Height;
            //_height = 0.1f * Height;

            var settingsButton = new SimpleButton(Width - _height / 2, _height / 2, _height, _height);
            AddChild(settingsButton);
            DeclareTappable(settingsButton);
            _y = SkiaRoot.ScreenHeight;
            settingsButton.Activated += SettingsButton_Tapped;
        }

        public Task Show()
        {
            this.Animate("slideIn", p => _y = (float)p, _y, SkiaRoot.ScreenHeight - _height, 8, (uint)300, Easing.CubicOut);
            return Task.Delay(300);
        }

        public Task Hide()
        {
            this.Animate("slideOut", p => _y = (float)p, _y, SkiaRoot.ScreenHeight, 8, (uint)300, Easing.CubicOut);
            return Task.Delay(300);
        }

        private void SettingsButton_Tapped()
        {
            SettingsButtonPressed?.Invoke();
        }

        protected override void Draw()
        {

        }
    }
}
