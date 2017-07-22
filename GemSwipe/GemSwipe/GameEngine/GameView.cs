using System;
using System.Threading.Tasks;
using GemSwipe.Models;
using SkiaSharp;

namespace GemSwipe.GameEngine
{
    public class GameView : SkiaView
    {
        private BoardFactoryView _boardFactoryView;
        private HeaderView _headerView;
        private BoardSetup _boardSetup;
        private bool _isBusy;
        private Random _randomizer;

        public GameView(BoardSetup boardSetup, SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _randomizer = new Random();
            _boardFactoryView = new BoardFactoryView(boardSetup, canvas, 0, 0, Height, width);

            _headerView = new HeaderView(canvas, 0, 0, (float)(0.1 * Height), width);
            _headerView.ZIndex = 2;

            AddChild(_boardFactoryView);
            AddChild(_headerView);
        }



        public void NextBoard(BoardSetup boardSetup)
        {
            _boardFactoryView.MoveTo(_randomizer.Next(10), _randomizer.Next(10));

            Task.Run(async () =>
            {
                await Task.Delay(0);
                _boardFactoryView.BoardView.Populate(boardSetup.Gems);
            });

        }

        public void Update(SwipeResult swipeResult)
        {
            _boardFactoryView.BoardView.Update(swipeResult);
            Task.Run(async () =>
            {
                _isBusy = true;
                await Task.Delay(200);
                _isBusy = false;
            });
        }

        public void UpdateCountDown(double remainingSeconds)
        {
            _headerView.CountDownView.RemainingSeconds = remainingSeconds;
        }

        protected override void Draw()
        {

        }

        public bool IsBusy()
        {
            return _isBusy;
        }

        public override void Dispose()
        {
        }
    }
}