using System;
using System.Threading.Tasks;
using GemSwipe.BoardSolver;
using GemSwipe.Data;
using GemSwipe.Game.Effects;
using GemSwipe.Data.Level;
using GemSwipe.Game.Effects.Popped;
using GemSwipe.Game.Gestures;
using GemSwipe.Game.Models;
using GemSwipe.Game.Navigation.Pages;
using GemSwipe.Utilities.Sprites;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game
{
    public class GamePage:PageBase
    {
        private Scene _scene;
        private BlockedSensor _blockedSensor;
        private bool _isBlocked;
        private EffectLayer _effectLayer;
        private bool _isBusy;
        private int _level=1;
        private LevelRepository _levelRepository;

        public GamePage( float x, float y, float height, float width, LevelRepository levelRepository) : base( x, y, height, width)
        {
            _blockedSensor = new BlockedSensor();
            _levelRepository = levelRepository;
        }

        public void BackgroundNextBoard()
        {

        }

        public async void Start(int levelId)
        {
            await _scene.StartingFloor.Start();

            LevelConfiguration levelconfig = _levelRepository.GetLevelConfigurationById(Math.Min(5,Math.Max(levelId,0)));
            _scene.SetLevelConfig(levelconfig);
            await _scene.NextTransitionFloor(levelId);

            BackgroundNextBoard();
            _isBusy = false;
        }

        public async void Swipe(Direction direction)
        {
            if(IsBusy()) return;

            if (_scene.CurrentBoard != null)
            {
                var swipeResult = _scene.CurrentBoard.Swipe(direction);

                if (swipeResult.BoardWon)
                {
                    //_effectLayer.Explode();
                    _isBusy = true;
                    //_level++;
                    // Generate Floor
                    await Task.Delay(1000);

                    BackgroundNextBoard();
                    await _scene.EndFloor();
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
                            var blockedMessage = new PoppedText( Width / 2, Height / 2, 1000, 300, 300,
                                "Blocked",
                                Height / 10, CreateColor(255, 0, 0));
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


        public bool IsBusy()
        {
            return _isBusy;
        }


        protected override void Draw()
        {
        }

        protected override void OnActivated(object parameter = null)
        {
            var levelId = (int) parameter;
            _scene = new Scene( 0, 0, Height, Width);
            AddChild(_scene);


            _effectLayer = new EffectLayer( 0, 0, Height, Width);
            AddChild(_effectLayer, 3);

            Start(levelId);
            GestureEventHandler.Swipped += OnSwipped;
        }

        private void OnSwipped(Direction direction)
        {
            Swipe(direction);
        }

        protected override void OnDeactivated()
        {
            _scene.Dispose();
            _effectLayer.Dispose();

            GestureEventHandler.Swipped -= OnSwipped;
        }
    }
}
