using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Menu
{
    public class HeaderView : SkiaView
    {
        public CountDownView CountDownView { get; set; }
        public HeaderView(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            var countDownWidth = width * 0.2f;
            var countDownHeight = height;

            CountDownView = new CountDownView(canvas, width / 2 - countDownWidth / 2, y, countDownHeight, countDownWidth);
            AddChild(CountDownView);
        }

        protected override void Draw()
        {
            var cellColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(233, 35, 23, 33)
            };

            Canvas.DrawRect(
                SKRect.Create(
                    X,
                    Y,
                    Width,
                    Height),
                cellColor);
        }
    }
}
