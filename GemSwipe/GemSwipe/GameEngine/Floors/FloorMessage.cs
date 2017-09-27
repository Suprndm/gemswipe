using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.Menu;
using SkiaSharp;

namespace GemSwipe.GameEngine.Floors
{
    public class FloorMessage : TextBlock
    {
        public TextBlock Message { get; }

        public FloorMessage(SKCanvas canvas, float x, float y, string text, float size, SKColor color) : base(canvas, x, y, text, size, color)
        {
        }
    }
}
