using GemSwipe.GameEngine.Menu;
using SkiaSharp;

namespace GemSwipe.GameEngine.Game.Floors
{
    public class FloorMessage : TextBlock
    {
        public TextBlock Message { get; }

        public FloorMessage(SKCanvas canvas, float x, float y, string text, float size, SKColor color) : base(canvas, x, y, text, size, color)
        {
        }
    }
}
