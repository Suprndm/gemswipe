using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game.Floors
{
    public abstract class Floor:SkiaView
    {
        protected Floor( float x, float y, float height, float width) : base( x, y, height, width)
        {

        }

        protected override void Draw()
        {
         
        }
    }
}
