using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace GemSwipe.GameEngine
{
    public class CountDownView : SkiaView
    {
        public double RemainingSeconds { get; set; }

        public CountDownView(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
        }


        protected override void Draw()
        {
            var cellColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(259, 35, 35)
            };

            Canvas.DrawRect(
                SKRect.Create(
                    X,
                    Y,
                    Width,
                    Height),
                cellColor);

            using (var paint = new SKPaint())
            {
                paint.TextSize = Height / 2f;
                paint.IsAntialias = true;
                paint.Color = new SKColor(255, 255, 255, 255);

                Canvas.DrawText(RemainingSeconds.ToString("###"), X + Width / 2, Y + Height / 2, paint);
            }
        }
    }
}
