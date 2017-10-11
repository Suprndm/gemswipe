using GemSwipe.Game.Effects.BackgroundEffects;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game.Floors
{
    public class FloorMessage : TextBlock
    {
        public TextBlock Message { get; }

        public FloorMessage( float x, float y, string text, float size, SKColor color) : base( x, y, text, size, color)
        {
        }
    }
}
