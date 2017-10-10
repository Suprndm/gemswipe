using GemSwipe.Game.Effects.BackgroundEffects;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game.Floors
{
    public class FloorTitle : TextBlock
    {
        public TextBlock Title { get;}

        public FloorTitle( float x, float y, string text, float size, SKColor color) : base( x, y, text, size, color)
        {
        }
    }
}
