using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine;
using GemSwipe.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GemSwipe.Views
{
    public partial class GamePage : ContentPage
    {
        private bool _panJustBegun;
        private Game _game;
        bool pageIsActive;
        private bool _isInitiated;
        private GameView _gameView;



        public GamePage()
        {
            InitializeComponent();
        }

        private void SKGLView_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (_isInitiated)
            {
                e.Surface.Canvas.Clear(SKColors.White);
                _gameView.Render();
            }
            else
            {
                _gameView = new GameView(e.Surface.Canvas, 0, 0, (float)e.Surface.Canvas.ClipBounds.Width, (float)e.Surface.Canvas.ClipBounds.Width);
                var gameSetup = _game.Setup();
                _gameView.Setup(gameSetup);

                _isInitiated = true;
            }
        }

        protected override void OnAppearing()
        {
            _panJustBegun = true;

            _game = new Game();

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
            SKGLView.PaintSurface += SKGLView_PaintSurface;

            base.OnAppearing();
        }


        // USer controls
        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var d = Math.Sqrt(e.TotalX * e.TotalX + e.TotalY * e.TotalY);
            if (d > 25 && _panJustBegun)
            {
                _panJustBegun = false;
                if (e.TotalX > 0)
                {
                    if (e.TotalY > e.TotalX)
                        Swipe(Direction.Bottom);
                    else if (Math.Abs(e.TotalY) > e.TotalX)
                        Swipe(Direction.Top);
                    else
                        Swipe(Direction.Right);
                }
                else
                {
                    if (e.TotalY > Math.Abs(e.TotalX))
                        Swipe(Direction.Bottom);
                    else if (Math.Abs(e.TotalY) > Math.Abs(e.TotalX))
                        Swipe(Direction.Top);
                    else
                        Swipe(Direction.Left);
                }
            }
        }

        private void Swipe(Direction direction)
        {
            var gameUpdate = _game.Swipe(direction);
            _gameView.Update(gameUpdate);

            if (gameUpdate.IsWon)
            {
                WinPopup.TranslateTo(0, 0, 500, Easing.CubicOut);
            }
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            _panJustBegun = true;
        }

        protected override void OnDisappearing()
        {
            SKGLView.PaintSurface -= SKGLView_PaintSurface;
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        private void Dispose()
        {
            _gameView.Dispose();
        }
    }
}
