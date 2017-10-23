using System;
using System.Threading.Tasks;
using GemSwipe.BoardSolver;
using GemSwipe.Data;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Effects;
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
        private bool _isBlocked;
        private EffectLayer _effectLayer;
        private bool _isBusy;
        private int _level=1;
        private LevelDataRepository _levelDataRepository;

        public GamePage( float x, float y, float height, float width) : base( x, y, height, width)
        {
            _levelDataRepository = new LevelDataRepository();
        }

        public void BackgroundNextBoard()
        {

        }

        public async void Start(int levelId)
        {
            await _scene.StartingFloor.Start();

            LevelData levelconfig = _levelDataRepository.Get(Math.Min(5,Math.Max(levelId,0)));
            _scene.SetLevelData(levelconfig);
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
                _isBusy = true;
                await Task.Delay(600);
                _isBusy = false;
                _scene.CurrentBoard.RefillGems();
                if (swipeResult.BoardWon)
                {
                    _isBusy = true;
                    await Task.Delay(1000);

                    BackgroundNextBoard();
                    await _scene.EndFloor();
                }
                //else if (!_isBlocked)
                //{
                //    var isBlocked = await _blockedSensor.IsBlocked(_scene.CurrentBoard.ToString());
                //    if (isBlocked)
                //    {
                //        _isBlocked = true;

                //        await Task.Run(async () =>
                //        {
                //            await Task.Delay(2000);
                //            _isBusy = true;
                //            var blockedMessage = new PoppedText( Width / 2, Height / 2, 1000, 300, 300,
                //                "Blocked",
                //                Height / 10, CreateColor(255, 0, 0));
                //            AddChild(blockedMessage);
                //            await blockedMessage.Pop();

                //            await _scene.ResetBoard();
                //            _isBusy = false;
                //            _isBlocked = false;

                //        });
                //    }
                //}
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
            AddChild(_effectLayer);

            Start(levelId);
            Gesture.Swipe += OnSwipped;
        }

        private void OnSwipped(Direction direction)
        {
            Swipe(direction);
        }

        protected override void OnDeactivated()
        {
            _scene.Dispose();
            _effectLayer.Dispose();

            Gesture.Swipe -= OnSwipped;
        }
    }
}
