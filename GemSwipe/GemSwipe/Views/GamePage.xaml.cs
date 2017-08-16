using System;
using System.Collections.Generic;
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
        bool pageIsActive;
        private bool _isInitiated;
        private Game _game;
        private SKCanvas _canvas;

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
            _game.Swipe(direction);
        }

        #endregion


        #region Render


        private void SKGLView_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (_isInitiated)
            {
                e.Surface.Canvas.Clear(SKColors.Black);
                _game.Render();
            }
            else
            {
                // Init SkiaSharp
                var gameSetup = SetupGame();
                _canvas = e.Surface.Canvas;
                _game = new Game(gameSetup, e.Surface.Canvas, 0, 0, e.Surface.Canvas.ClipBounds.Height, e.Surface.Canvas.ClipBounds.Width);
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

        private GameSetup SetupGame()
        {
            return new GameSetup(1, new List<BoardSetup>
            {
                new BoardSetup(4,4,"1 1 1 1-0 0 0 0-0 0 0 0-0 0 0 0"),
                new BoardSetup(4,4,"1 1 1 1-0 0 0 0-1 0 1 0-1 0 1 0"),
                new BoardSetup(4,4,"1 1 0 1-0 1 0 1-0 0 0 0-1 0 1 1"),
                new BoardSetup(4,4,"1 1 2 1-0 1 2 1-0 0 2 2-1 0 1 1"),
                new BoardSetup(4,4,"1 1 2 1-0 1 2 1-0 0 2 2-1 0 1 1"),
            });
        }

        private void SetupSkiaView()
        {
            _panJustBegun = true;
            SKGLView.PaintSurface += SKGLView_PaintSurface;

            Gestures.Gesture.SetTapped(SKGLView, new Command<Point>(OnCanvasTapped));
        }

        private void OnCanvasTapped(Point p)
        {
         
            _game.DetectTap(p);;
            _panJustBegun = true;
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
            if (e.TotalX == 0 && e.TotalY == 0) return;

            var eX = e.TotalX;
            var eY = e.TotalY;
            var d = Math.Sqrt(eX * eX + eY * eY);

            if (d > 25 && !_game.IsBusy() && _panJustBegun)
            {
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

        protected override void OnDisappearing()
        {
            SKGLView.PaintSurface -= SKGLView_PaintSurface;
        }

        #endregion

        private void Dispose()
        {
            _game.Dispose();
        }
    }
}