using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Physical
{
    public class ParticuleView:SkiaView
    {
        public ParticuleView(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(255, 255, 255, 255);
                Canvas.DrawCircle(X, Y, Height, paint);
            }
        }
    }
}
