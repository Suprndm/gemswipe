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
        }
    }
}
