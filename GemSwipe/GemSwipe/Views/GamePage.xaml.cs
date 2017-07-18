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

        public GamePage()
        {
            InitializeComponent();
          
        }

        protected override void OnAppearing()
        {
            Task.Run(() => { Task.Delay(0); Doors.Open();
            });

            Task.Run(() => {
                Task.Delay(0);
                GameView.IsVisible = true;
            });

            _panJustBegun = true;

            _game = new Game();
            var gameSetup = _game.Setup();
            GameView.Setup(gameSetup);

            WinPopup.TranslateTo(-500, 0, 0, Easing.CubicOut);
            GameView.TranslateTo(0, -500, 0, Easing.CubicOut);
            GameView.TranslateTo(0, 0, 500, Easing.CubicOut);
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
            GameView.Update(gameUpdate);

            if (gameUpdate.IsWon)
            {
                Doors.Close();

                GameView.TranslateTo(0, -500, 0, Easing.CubicOut);
                WinPopup.TranslateTo(0, 0, 500, Easing.CubicOut);

                Task.Delay(500);
                GameView.IsVisible = false;
            }
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            _panJustBegun = true;
        }

        protected override void OnDisappearing()
        {
            Doors.Close();
        }

        protected override bool OnBackButtonPressed()
        {
            Doors.Close();
            return false;
        }

        private void Dispose()
        {
            GameView.Dispose();
        }
    }
}
