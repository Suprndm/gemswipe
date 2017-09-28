using GemSwipe.GameEngine.Menu;
using SkiaSharp;

namespace GemSwipe.GameEngine.Game.Floors
{
    public class FloorTitle : TextBlock
    {
        public TextBlock Title { get;}

        public FloorTitle(SKCanvas canvas, float x, float y, string text, float size, SKColor color) : base(canvas, x, y, text, size, color)
        {
        }
    }
}
