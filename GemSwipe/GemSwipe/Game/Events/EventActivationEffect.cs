using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Effects;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Events
{
    public class EventActivationEffect : SkiaView
    {
        private float _maxRadius;
        private float _radius;
        public EventActivationEffect(float x, float y, float height, float width) : base(x, y, height, width)
        {
            _radius = width/2;
            _maxRadius = _radius * 4f;
            _opacity = 0;
        }

        public async Task Start()
        {
            this.Animate("opacity", p => _opacity = (float)p, 1, 0, 4, 500, Easing.SinIn);

            this.Animate("radius", p => _radius = (float)p, _radius, _maxRadius, 4, 500, Easing.SinOut);

            await Task.Delay(500);

            Dispose();
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = CreateColor(255, 255, 255, (byte)(255 * _opacity));
                paint.Style =SKPaintStyle.Stroke;
                paint.StrokeWidth = 4;
                Canvas.DrawCircle(X, Y, _radius, paint);
            }
        }
    }
}
