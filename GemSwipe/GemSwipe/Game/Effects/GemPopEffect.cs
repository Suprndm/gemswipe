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

            AddChild(new ExplosionEffect(0,0,Height,Width,50,10,0.95f));
            this.Animate("radius2", p => _radius = (float)p, _maxRadius/8, _maxRadius/2, 4, 800, Easing.SinOut);
            await Task.Delay(10000);

            Dispose();
        }

        protected override void Draw()
        {
            //var colors = new SKColor[] {
            //    CreateColor (255, 255, 255,0),
            //    CreateColor (255, 255,255, (byte)(75* _invertedOpacity)),
            //    CreateColor (255, 255, 255,0),
            //    CreateColor (255, 255,255, (byte)(75* _invertedOpacity)),
            //    CreateColor (255, 255, 255,0),
            //    CreateColor (255, 255,255, (byte)(75* _invertedOpacity)),
            //    CreateColor (255, 255, 255,0),
            //    CreateColor (255, 255,255, (byte)(75* _invertedOpacity)),
            //    CreateColor (255, 255, 255,0),
            //};

            //var haloRadius = _radius * 1f;
            //var shader = SKShader.CreateRadialGradient(new SKPoint(X, Y), haloRadius, colors, new[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f,0.9f }, SKShaderTileMode.Clamp);
            //var glowPaint = new SKPaint()
            //{
            //    Shader = shader,
            //};
            //Canvas.DrawCircle(X, Y, haloRadius, glowPaint);

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

                Canvas.DrawCircle(X, Y, _maxRadius/4, paint);
            }
        }
    }
}
