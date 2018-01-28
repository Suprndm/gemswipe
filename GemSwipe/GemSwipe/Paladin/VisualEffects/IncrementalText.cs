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
    public class IncrementalText : ToastText
    {
        private float _incrementalValue;
        private float _targetValue;
        private float _initialValue;

        public IncrementalText(float initialValue, float targetValue, float x, float y, float size,
            SKColor color, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center) : base(x, y, string.Empty,
            size, color, horizontalAlignment)
        {
            _targetValue = targetValue;
            _initialValue = initialValue;
            _incrementalValue = _initialValue;
        }

        public Task Start()
        {
            Show();
            this.Animate("incrementalText", p => _incrementalValue = (float) p, _initialValue, _targetValue, 16,
                (uint) 800, Easing.CubicOut);

            return Task.Delay(800);
        }

        protected override void Draw()
        {
            Text = Math.Round(_incrementalValue).ToString();
            base.Draw();
        }
    }
}
