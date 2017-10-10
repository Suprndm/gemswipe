﻿using System.Threading.Tasks;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings
{
    public class TopBar:SkiaView
    {
        public TopBar(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _height = 0.1f * height;

            var settingsButton = new SimpleButton(canvas, width -_height/2, _height/2, _height, _height,
                CreateColor(255, 255, 255));
            AddChild(settingsButton);
            DeclareTappable(settingsButton);
            _y = -height;
            settingsButton.Tapped += SettingsButton_Tapped;
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
            Navigator.Instance.ShowSettings();
        }

        protected override void Draw()
        {
            DrawHitbox();

        }
    }
}