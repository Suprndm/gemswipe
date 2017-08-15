using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.BoardSolver;
using GemSwipe.GameEngine.Menu;
using GemSwipe.GameEngine.Popped;
using GemSwipe.GameEngine.SkiaEngine;
using GemSwipe.Models;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.GameEngine
{
    public class Game : SkiaView
    {
        private Scene _scene;
        private HeaderView _headerView;
        private GameSetup _gameSetup;
        private CountDown _countDown;
        private BlockedSensor _blockedSensor;

        private bool _isBusy;

        public Game(GameSetup gameSetup, SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _scene = new Scene(gameSetup, canvas, 0, 0, Height, width);

            _headerView = new HeaderView(canvas, 0, 0, (float)(0.1 * Height), width);
            _headerView.ZIndex = 2;

            _blockedSensor = new BlockedSensor();

            AddChild(_scene);
            //   AddChild(_headerView);

            var tapToPlay = new TapToPlay(canvas, 0, 0, height, width);
            AddChild(tapToPlay);
            tapToPlay.Tapped += Start;
        }

        public async void Start()
        {
            await _scene.StartingFloor.Start();
            await _scene.NextFloor();

            _countDown = new CountDown(15);
            _countDown.Zero += () =>
           {
               //   Lost?.Invoke(new GameLostData { MaxLevel = 5, Score = 65494 });
           };
            _countDown.Start();
        }

        public void NextBoard(BoardSetup boardSetup)
        {
            _scene.NextFloor();
        }

        public async void Swipe(Direction direction)
        {
            if (_scene.CurrentBoard != null)
            {
                _isBusy = true;
                var swipeResult = _scene.CurrentBoard.Swipe(direction);


                if (swipeResult.BoardWon)
                {
                    await Task.Delay(300);
                    _countDown.AddMoreTime(8);
                    await _scene.NextFloor();
                    _isBusy = false;
                }
                else
                {
                    var isBlocked = await _blockedSensor.IsBlocked(_scene.CurrentBoard.ToString());
                    if (isBlocked)
                    {
                        var blockedMessage = new PoppedText(Canvas, Width / 2, Height / 2, 1000, 300, 300, "Blocked",
                            Height / 10, new SKColor(255, 0, 0));
                        AddChild(blockedMessage);
                        await blockedMessage.Pop();

                        await _scene.ResetBoard();
                    }

                    _isBusy = false;
                }

                if (swipeResult.GameFinished)
                {

                }

            }
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