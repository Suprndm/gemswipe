using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace GemSwipe.GameEngine
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
