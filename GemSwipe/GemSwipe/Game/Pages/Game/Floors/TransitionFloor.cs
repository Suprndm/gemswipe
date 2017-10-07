using System.Threading.Tasks;
using GemSwipe.Data;
using GemSwipe.Data.Level;
using GemSwipe.Game.Models;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game.Floors
{

    public class TransitionFloor : Floor
    {
        public FloorTitle Title { get; }
        public FloorMessage Quote { get; }
        public string Msg { get; set; }

        public TransitionFloor(SKCanvas canvas, float x, float y, float height, float width, TransitionFloorSetup setup) : base(canvas, x, y, height, width)
        {
            var boardMarginTop = height * 0.2f;
            SKColor color = CreateColor(255, 255, 255, 255);

            Title = new FloorTitle(canvas, Width / 2, Height / 10, setup.Title, Height / 20, color);
            AddChild(Title);

            Quote = new FloorMessage(canvas, Width / 2, Height / 2, setup.Quote, Height / 20, color);
            AddChild(Quote);

            DeclareTappable(this);
        }

        public TransitionFloor(SKCanvas canvas, float x, float y, float height, float width, LevelConfiguration setup) : base(canvas, x, y, height, width)
        {
            var boardMarginTop = height * 0.2f;
            SKColor color = CreateColor(255, 255, 255, 255);

            Title = new FloorTitle(canvas, Width / 2, Height / 10, setup.Title, Height / 20, color);
            AddChild(Title);

            Quote = new FloorMessage(canvas, Width / 2, Height / 2, setup.Story, Height / 20, color);
            AddChild(Quote);

            DeclareTappable(this);
        }
    }
}
