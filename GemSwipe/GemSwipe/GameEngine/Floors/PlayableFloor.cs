using GemSwipe.GameEngine.Menu;
using GemSwipe.Models;
using SkiaSharp;

namespace GemSwipe.GameEngine.Floors
{
    public class PlayableFloor : Floor
    {
        private readonly PlayableFloorSetup _setup;
        public Board Board { get; }

        public PlayableFloor(SKCanvas canvas, float x, float y, float height, float width, PlayableFloorSetup setup) : base(canvas, x, y, height, width)
        {
            var boardMarginTop = height * 0.2f;
            var board = new Board(setup.BoardSetup, canvas, 0, 0 + boardMarginTop, width, width);
            AddChild(board);
            Board = board;
            _setup = setup;

            var text = $"Floor { _setup.Floor}";
            if (_setup.IsFinal)
                text = "Final Floor !";

            AddChild(new TextBlock(Canvas, Width / 2, Height * .15f, text, (int)Width / 10, new SKColor(255, 255, 255, 255)));

        }
    }
}
