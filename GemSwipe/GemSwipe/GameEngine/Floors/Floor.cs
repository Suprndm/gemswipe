using System;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Floors
{
    public abstract class Floor:SkiaView
    {
        protected Floor(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {

        }

        protected override void Draw()
        {
            //using (var paint = new SKPaint())
            //{
            //    paint.IsAntialias = true;
            //    paint.Color = new SKColor(255, 255, 255, 255);

            //    Canvas.DrawRect(
            //        SKRect.Create(
            //            X+Width/2,
            //            Y,
            //            2,
            //            Height),
            //        paint);
            //}
        }
    }
}
