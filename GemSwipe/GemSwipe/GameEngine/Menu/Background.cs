using System;
using GemSwipe.GameEngine.Effects;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Menu
{
    public class Background : SkiaView
    {
        public Background(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
         
        }

        protected override void Draw()
        {
            var rectangleHeight = Height / 600;
            var rectangleCount = (int)(Height / rectangleHeight);
            for (int i = 0; i < rectangleCount; i++)
            {
                using (var paint = new SKPaint())
                {
                    var colorModificator = (byte)(i % 2 * 10);
                    paint.IsAntialias = true;
                    paint.Color = new SKColor((byte)(26 - colorModificator), (byte)(53 - colorModificator), (byte)(80 - colorModificator), 255);
                    Canvas.DrawRect(SKRect.Create(X, Y + i * rectangleHeight, Width, rectangleHeight), paint);
                }
            }
        }
    }
}
