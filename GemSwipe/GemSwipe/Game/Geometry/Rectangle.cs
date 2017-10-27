using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Geometry
{
    public class Rectangle:SkiaView
    {
        private SKColor _color;
        public Rectangle(float x, float y, float height, float width, SKColor color) : base(x, y, height, width)
        {
            _color = color;
        }
        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Style = SKPaintStyle.Fill;
                paint.Color = CreateColor(_color.Red, _color.Green, _color.Blue, (byte)(_opacity * 255));
                Canvas.DrawRect(SKRect.Create(X, Y, Width, Height), paint);
            }
        }
    }
}
