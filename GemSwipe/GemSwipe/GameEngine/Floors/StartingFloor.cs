using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.Menu;
using GemSwipe.GameEngine.Popped;
using SkiaSharp;

namespace GemSwipe.GameEngine.Floors
{
    public class StartingFloor : Floor
    {
        private readonly int _level;
        public StartingFloor(SKCanvas canvas, float x, float y, float height, float width, int level) : base(canvas, x, y, height, width)
        {
            _level = level;

            AddChild(new TextBlock(Canvas, Width / 2, Height * .05f, $"Level", (int)Width / 10, new SKColor(255, 255, 255, 255)));
            AddChild(new TextBlock(Canvas, Width / 2, Height * .2f, _level.ToString(), (int)Width / 5, new SKColor(255, 255, 255, 255)));
        }

        public async Task Start()
        {
            await Task.Delay(200);

            var readyText = new PoppedText(Canvas, Width / 2, Height / 2, 1800, 500, 100, "Ready ?", 0.10f * Height, new SKColor(255, 255, 255, 255));
            AddChild(readyText);

            var goText = new PoppedText(Canvas, Width / 2, Height / 2, 100, 100, 300, "Go !", 0.2f * Height, new SKColor(255, 255, 255, 255));
            AddChild(goText);

            await readyText.Pop();
            await goText.Pop();
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(23, 94, 165, 255);

                var radius = Width * 2.5f;
                Canvas.DrawCircle(X + Width / 2, Y - 9 * radius / 10, radius, paint);
            }
        }
    }
}
