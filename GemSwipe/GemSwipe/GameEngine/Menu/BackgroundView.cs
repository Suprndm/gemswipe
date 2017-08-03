using System;
using GemSwipe.GameEngine.Physical;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Menu
{
    public class BackgroundView :SkiaView
    {
        private const int ParticuleNumbers = 1;
        public BackgroundView(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            var randomizer = new Random();
            for (int i = 0; i < ParticuleNumbers; i++)
            {
                AddChild(new ParticuleView(canvas, (float)randomizer.Next((int)width),(float)randomizer.Next((int)height),height/3000, height / 3000));
            }
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(50, 50, 50, 255);

                Canvas.DrawRect(
                    SKRect.Create(
                        X,
                        Y,
                        Width,
                        Height),
                    paint);
            }

        }
    }
}
