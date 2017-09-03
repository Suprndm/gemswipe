using System;
using GemSwipe.GameEngine.Effects;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Menu
{
    public class Background : SkiaView
    {
        public Background(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height,
            width)
        {

        }

        protected override void Draw()
        {
            var colors = new SKColor[]
            {
                new SKColor(71, 9, 196, 255),
                new SKColor(119, 74, 212, 255),
            };
            var shader = SKShader.CreateLinearGradient(new SKPoint(0, 0), new SKPoint(0, Height), colors, null,
                SKShaderTileMode.Clamp);
            var paint = new SKPaint() {Shader = shader};
            paint.IsAntialias = true;
            Canvas.DrawRect(SKRect.Create(X, Y, Width, Height), paint);
        }
    }
}
