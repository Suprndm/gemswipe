using System.Threading.Tasks;
using GemSwipe.Models;
using SkiaSharp;

namespace GemSwipe.GameEngine
{
    public class GameView : SkiaView
    {
        private BoardView _boardView;
        private BoardSetup _boardSetup;
        private double _remainingSeconds;
        private bool _isBusy;

        public GameView(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
        }

        public void SetupNewBoard(BoardSetup boardSetup)
        {
            _boardSetup = boardSetup;

            _boardView = new BoardView(Canvas, 0, (float)(0.1 * Height), Canvas.ClipBounds.Width, Canvas.ClipBounds.Width);
            _boardView.Setup(_boardSetup.Columns, _boardSetup.Rows);
            _boardView.Populate(_boardSetup.Gems);

            AddChild(_boardView);
        }

        public void Update(SwipeResult swipeResult)
        {
            _boardView.Update(swipeResult);
            Task.Run(async () =>
            {
                _isBusy = true;
                await Task.Delay(200);
                _isBusy = false;
            });
        }

        public void UpdateCountDown(double remainingSeconds)
        {
            _remainingSeconds = remainingSeconds;
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = 64.0f;
                paint.IsAntialias = true;
                paint.Color = new SKColor(0, 0, 0);
                paint.IsStroke = true;

                Canvas.DrawText(_remainingSeconds.ToString("###.#"), Width / 2, (float)0.05 * Height, paint);
            }
        }

        public bool IsBusy()
        {
            return _isBusy;
        }

        public override void Dispose()
        {
            _boardView.Dispose();
        }
    }
}