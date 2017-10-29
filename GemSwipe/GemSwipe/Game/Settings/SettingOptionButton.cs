using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.UIElements.Buttons;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings
{
    public class SettingOptionButton : TextButton
    {
        private byte _textOpacity;
        private int _animationMs = 600;

        public SettingOptionButton(float x, float y, float height,string text) : base(x, y, height, text)
        {
            TextColor = new SKColor(255,255,255,0);
        }

        public Task LightText()
        {
            this.Animate("LightText", p => _textOpacity = (byte)p, _textOpacity, 255, 8, (uint)_animationMs,
                Easing.CubicInOut);
            return Task.Delay(_animationMs);
        }

        public Task FadeText()
        {
            this.Animate("FadeText", p => _textOpacity = (byte)p, _textOpacity, 0, 8, (uint)_animationMs,
                Easing.CubicInOut);
            return Task.Delay(_animationMs);
        }
        

    }
}
