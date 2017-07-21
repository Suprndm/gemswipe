using System;
using GemSwipe.GameEngine;
using GemSwipe.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace GemSwipe.Views
{
    public partial class GamePage : ContentPage
    {
        private bool _panJustBegun;
        private Game _game;
        bool pageIsActive;
        private bool _isInitiated;
        private GameView _gameView;
        private Point _chainedSwipeLastMove;


        public GamePage()
        {
            InitializeComponent();
        }

        #region Lose

        private void _game_Lost(GameLostData obj)
        {
            WinPopup.TranslateTo(0, 0, 500, Easing.CubicOut);
        }

        #endregion

        #region Swipe

        private void Swipe(Direction direction)
        {
            var gameUpdate = _game.Swipe(direction);
            _gameView.Update(gameUpdate);

            if (gameUpdate.IsWon)
            {
                NextBoard();
            }
        }

        #endregion

        #region NextBoard

        public void NextBoard()
        {

        }

        #endregion

        #region Render

        private void UpdateViewModel()
        {
            _gameView.UpdateCountDown(_game.SecondsTillLose());
        }

        private void SKGLView_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (_isInitiated)
            {
                e.Surface.Canvas.Clear(SKColors.White);
                UpdateViewModel();
                _gameView.Render();
            }
            else
            {
                // Init SkiaSharp
                _gameView = new GameView(e.Surface.Canvas, 0, 0, e.Surface.Canvas.ClipBounds.Width, e.Surface.Canvas.ClipBounds.Width);
                var gameSetup = _game.SetupBoard();
                _game.Start();
                _gameView.SetupNewBoard(gameSetup);

                _isInitiated = true;
            }
        }

        #endregion

        #region Setup

        protected override void OnAppearing()
        {
            SetupGame();
            SetupEndMenu();
            SetupSkiaView();

            base.OnAppearing();
        }

        private void SetupGame()
        {
            _game = new Game();
            _game.Lost += _game_Lost;
        }

        private void SetupSkiaView()
        {
            _panJustBegun = true;
            SKGLView.PaintSurface += SKGLView_PaintSurface;
        }

        private void SetupEndMenu()
        {
            WinPopup.TranslateTo(-500, 0, 0, Easing.CubicOut);

            WinPopup.Back += () =>
            {
                Navigation.InsertPageBefore(new MenuPage(), this);
                Navigation.PopAsync();
                Dispose();
            };

            WinPopup.Retry += () =>
            {
                Navigation.InsertPageBefore(new GamePage(), this);
                Navigation.PopAsync();
                Dispose();
            };

            WinPopup.Next += () =>
            {
                Navigation.InsertPageBefore(new GamePage(), this);
                Navigation.PopAsync();
                Dispose();
            };
        }

        #endregion

        #region UserControls

        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if(e.TotalX==0 && e.TotalY==0) return;

            var eX = e.TotalX - _chainedSwipeLastMove.X;
            var eY = e.TotalY - _chainedSwipeLastMove.Y;
            var d = Math.Sqrt(eX * eX + eY * eY);

            if (d > 25 && !_gameView.IsBusy())
            {
                _chainedSwipeLastMove = new Point(e.TotalX, e.TotalY);
                _panJustBegun = false;
                if (eX > 0)
                {
                    if (eY > eX)
                        Swipe(Direction.Bottom);
                    else if (Math.Abs(eY) > eX)
                        Swipe(Direction.Top);
                    else
                        Swipe(Direction.Right);
                }
                else
                {
                    if (eY > Math.Abs(eX))
                        Swipe(Direction.Bottom);
                    else if (Math.Abs(eY) > Math.Abs(eX))
                        Swipe(Direction.Top);
                    else
                        Swipe(Direction.Left);
                }
            }
        }


        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            _chainedSwipeLastMove = new Point(0, 0);
            _panJustBegun = true;
        }

        protected override void OnDisappearing()
        {
            SKGLView.PaintSurface -= SKGLView_PaintSurface;
        }

        #endregion

        private void Dispose()
        {
            _gameView.Dispose();
        }
    }
}