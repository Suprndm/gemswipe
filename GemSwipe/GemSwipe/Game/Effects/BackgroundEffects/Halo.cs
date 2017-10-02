using System;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class Halo : SkiaView
    {
        private const double Speed = 0.005;
        public double Angle { get; private set; }
        public SKColor Color { get; }

        public Halo(SKCanvas canvas, float x, float y, float height, float width, SKColor color, double angle) : base(canvas, x, y, height, width)
        {
            Color = color;
            Angle = angle;
        }

        protected override void Draw()
        {
            Angle += Speed;

            _y = (float)Math.Sin(Angle) * Height*0.8f + Height / 2;
            _x = (float)Math.Cos(Angle) * Width/3 + Width / 6;

            var colors = new SKColor[] {
                new SKColor(Color.Red, Color.Green, Color.Blue, 50),
                new SKColor(Color.Red, Color.Green, Color.Blue, 0),
            };

            var shader = SKShader.CreateRadialGradient(new SKPoint(X, Y), Width / 2, colors, new[] { 0.0f, 1f }, SKShaderTileMode.Clamp);

            var paint = new SKPaint() { Shader = shader };
            Canvas.DrawCircle(X, Y, Width / 2, paint);

        }
    }
}

