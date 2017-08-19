using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.Menu;
using SkiaSharp;

namespace GemSwipe.GameEngine.Floors
{
    public class EndFloor : Floor
    {
        public EndFloor(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            AddChild(new TextBlock(Canvas, Width / 2, Height * .5f, $"Game Over", (int)Width / 10, new SKColor(255, 255, 255, 255)));
        }

        protected override void Draw()
        {
            base.Draw();
        }
    }
}
