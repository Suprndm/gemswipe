using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.BoardSolver;
using GemSwipe.Data;
using GemSwipe.GameEngine.Effects;
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
        private BlockedSensor _blockedSensor;
        private readonly BoardRepository _boardRepository;
        private readonly Background _background;
        private bool _isBlocked;
        private EffectLayer _effectLayer;
        private bool _isBusy;
        private int _level;

        public Game(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _level = 1;
            _boardRepository = new BoardRepository();
            _blockedSensor = new BlockedSensor();
            _background = new Background(canvas, 0, 0, height, width);
            AddChild(_background, -1);

            _scene = new Scene(canvas, 0, 0, Height, width);
            AddChild(_scene);


            _effectLayer = new EffectLayer(Canvas, 0, 0, Height, Width);
            AddChild(_effectLayer, 3);

            var tapToPlay = new TapToPlay(canvas, 0, 0, height, width);
            AddChild(tapToPlay);
            tapToPlay.Tapped += Start;
        }

        public async void Restart()
        {
            _scene.Reset();
        }

        public async void Start()
        {

            await _scene.StartingFloor.Start();
            _background.OnNextBoard();
            await _scene.NextBoard(_boardRepository.GetRandomBoardSetup(_level));
            _isBusy = false;
        }

        public async Task EndGame()
        {
            _isBusy = true;
            await _scene.EndGame();
            var tapToPlay = new TapToPlay(Canvas, 0, 0, Height, Width);
            AddChild(tapToPlay);
            tapToPlay.Tapped += Start;
        }

        public async void Swipe(Direction direction)
        {
            if (_scene.CurrentBoard != null)
            {

                var swipeResult = _scene.CurrentBoard.Swipe(direction);


                if (swipeResult.BoardWon)
                {
                    //_effectLayer.Explode();
                    _isBusy = true;
                    // Generate Floor
                    await Task.Delay(1000);

                    _background.OnNextBoard();
                    await _scene.NextBoard(_boardRepository.GetRandomBoardSetup(_level));
                    _isBusy = false;
                }
                else if (!_isBlocked)
                {
                    var isBlocked = await _blockedSensor.IsBlocked(_scene.CurrentBoard.ToString());
                    if (isBlocked)
                    {
                        _isBlocked = true;

                        await Task.Run(async () =>
                        {
                            await Task.Delay(2000);
                            _isBusy = true;
                            var blockedMessage = new PoppedText(Canvas, Width / 2, Height / 2, 1000, 300, 300,
                                "Blocked",
                                Height / 10, new SKColor(255, 0, 0));
                            AddChild(blockedMessage);
                            await blockedMessage.Pop();

                            await _scene.ResetBoard();
                            _isBusy = false;
                            _isBlocked = false;

                        });
                    }
                }

                if (swipeResult.GameFinished)
                {

                }

            }
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