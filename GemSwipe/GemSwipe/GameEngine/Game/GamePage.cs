using System.Threading.Tasks;
using GemSwipe.BoardSolver;
using GemSwipe.Data;
using GemSwipe.GameEngine.Effects;
using GemSwipe.GameEngine.Navigation.Pages;
using GemSwipe.GameEngine.Popped;
using GemSwipe.Gestures;
using GemSwipe.Models;
using SkiaSharp;

namespace GemSwipe.GameEngine.Game
{
    public class GamePage:PageBase
    {
        private Scene _scene;
        private BlockedSensor _blockedSensor;
        private bool _isBlocked;
        private EffectLayer _effectLayer;
        private bool _isBusy;
        private int _level;
        private BoardRepository _boardRepository;

        public GamePage(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _blockedSensor = new BlockedSensor();
            _boardRepository = new BoardRepository();
        }

        public void BackgroundNextBoard()
        {

        }

        public async void Start()
        {
            await _scene.StartingFloor.Start();

            _scene.SetupBoard = _boardRepository.GetRandomBoardSetup(_level);
            await _scene.NextTransitionBoard();
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


        public bool IsBusy()
        {
            return _isBusy;
        }


        protected override void Draw()
        {
        }

        protected override void OnActivated()
        {
            _level = 1;

            _scene = new Scene(Canvas, 0, 0, Height, Width);
            AddChild(_scene);


            _effectLayer = new EffectLayer(Canvas, 0, 0, Height, Width);
            AddChild(_effectLayer, 3);

            Start();
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
