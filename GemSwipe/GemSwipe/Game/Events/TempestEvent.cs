
using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.Game.Entities;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Events
{
    public class TempestEvent : EventBase
    {
        public TempestEvent(float x, float y, float height, float width) : base(x, y, height, width)
        {
            Color = new SKColor(176, 92, 255);
        }

        public override IList<Point> GetTargets(Board board)
        {
            return new List<Point>();
        }

        public override async Task<bool> Activate(Board board)
        {
            await base.Activate(board);

            var isFull = await board.RefillGems();
            isFull = await board.RefillGems();
            isFull = await board.RefillGems();
            isFull = await board.RefillGems();

            return !isFull;
        }
    }
}
