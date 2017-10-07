using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game.Floors
{
    public abstract class Floor:SkiaView
    {
        protected Floor(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {

        }

        protected override void Draw()
        {
         
        }
    }
}
