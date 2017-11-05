using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.UIElements.Buttons;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings.Options
{
    public class OptionPickerButton : TextButton
    {
        private byte _textOpacity;
        private int _animationMs = 600;

        public OptionPickerButton(float x, float y, float height, string text) : base(x, y, height, text)
        {
            Up += () => LightText();
        }

        public Task LightText()
        {
            this.Animate("LightText", p => TextColor = new SKColor(255, 255, 255, (byte)p), 0, 255, 8, (uint)_animationMs,
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
