using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Effects
{
    public class GemPopEffect : SkiaView
    {
        private float _radius;
        private float _maxRadius;
        private float _opacity;
        private float _invertedOpacity;

        public GemPopEffect(float x, float y, float height, float width) : base(x, y, height, width)
        {
            _maxRadius = width ;
            _radius = 0;
            _opacity = 1;
        }

        public async Task Start()
        {
            Task.Factory.StartNew(async () =>
            {
                this.Animate("invertedOpacity", p => _invertedOpacity = (float)p, 0, 1, 4, 500, Easing.SinIn);

                await Task.Delay(550);
                this.Animate("invertedOpacity2", p => _invertedOpacity = (float)p, 1, 0, 4, 800, Easing.SinInOut);
            });

            this.Animate("radius", p => _radius = (float)p, _maxRadius, 0, 4, 500, Easing.SinIn);

            await Task.Delay(400);

            AddChild(new ExplosionEffect(0,0,Height,Width,50,Height/8,0.95f));
            this.Animate("radius2", p => _radius = (float)p, _maxRadius/8, _maxRadius*0.8f, 4, 800, Easing.SinOut);
            await Task.Delay(10000);

            Dispose();
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.StrokeWidth = 4;
                paint.Style = SKPaintStyle.Stroke;
                paint.Color = CreateColor(255, 255, 255, (byte)(255 * _invertedOpacity));

                Canvas.DrawCircle(X, Y, _radius, paint);
            }

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Style = SKPaintStyle.Fill;
                paint.Color = CreateColor(255, 255, 255, (byte)(255 * _invertedOpacity));

                Canvas.DrawCircle(X, Y, _maxRadius/2, paint);
            }
        }
    }
}
