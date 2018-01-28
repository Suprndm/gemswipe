using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.UIElements;
using GemSwipe.Paladin.Utilities;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Paladin.VisualEffects
{
    public class ToastText:TextBlock
    {
        private float _targetY;
        private float _targetOpacity;
        public ToastText(float x, float y, string text, float size, SKColor color, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center) : base(x, y, text, size, color, horizontalAlignment)
        {
            _opacity = 0;
        }

        public async Task Show()
        {
            _y = _y - Size * 2;

            _targetY = _y + Size * 2;

            this.Animate("toastHeight", p => _y = (float)p, _y, _targetY, 4, (uint)400, Easing.CubicOut);
            this.Animate("toastOpacity", p => _opacity = (float)p, 0, 1, 4, (uint)400, Easing.CubicOut);
            await Task.Delay(400);
        }
    }
}
