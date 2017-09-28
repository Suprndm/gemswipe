using GemSwipe.Models;
using SkiaSharp;

namespace GemSwipe.GameEngine.Game.Floors
{

    public class TransitionFloor : Floor
    {
        private readonly TransitionFloorSetup _setup;
        public FloorTitle Title { get; }
        public FloorMessage Quote { get; }

        public TransitionFloor(SKCanvas canvas, float x, float y, float height, float width, TransitionFloorSetup setup) : base(canvas, x, y, height, width)
        {

            var boardMarginTop = height * 0.2f;
           _setup = setup;

            SKColor color = new SKColor(255, 255, 255, 255);
            Title = new FloorTitle(canvas, Width / 2, Height / 10, setup.Title, Height / 20, color);
            AddChild(Title);

            Quote = new FloorMessage(canvas, Width / 2, Height / 2, setup.Quote, Height / 20, color);
            AddChild(Quote);

            DeclareTappable(this);
        }
    }
}
