using System.Collections.Generic;
using GemSwipe.Game.Entities;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Events
{
    public class EmptyEvent:EventBase
    {
        public EmptyEvent(float x, float y, float height, float width) : base(x, y, height, width)
        {
            Color = new SKColor(255, 255, 255,100);
        }

        public override IList<Point> GetTargets(Board board)
        {
            return new List<Point>();
        }
    }
}
