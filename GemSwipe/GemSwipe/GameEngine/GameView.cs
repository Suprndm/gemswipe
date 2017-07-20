using GemSwipe.Models;
using SkiaSharp;

namespace GemSwipe.GameEngine
{
    public class GameView : SkiaView
    {
        private BoardView _boardView;
        private GameSetup _gameSetup;

        public GameView(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
        }

        public void Setup(GameSetup gameSetup)
        {
            _gameSetup = gameSetup;

            _boardView = new BoardView(Canvas, 0, 0, (float)Canvas.ClipBounds.Width, (float)Canvas.ClipBounds.Width);
            _boardView.Setup(_gameSetup.Columns, _gameSetup.Rows);
            _boardView.Populate(_gameSetup.Gems);

            AddChild(_boardView);
        }

        public void Update(GameUpdate gameUpdate)
        {
            _boardView.Update(gameUpdate);
        }

        protected override void Draw()
        {
        }

        public override void Dispose()
        {
            _boardView.Dispose();
        }
    }
}