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
        private int _boardWidth;
        private int _boardHeight;

        public GamePage()
        {
            InitializeComponent();
            _game = new Game();
            _game.InitGame();
            _panJustBegun = true;

            GameScreen.InitDisplay(_game);
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
                        _game.Swipe(Direction.Bottom);
                    else if (Math.Abs(e.TotalY) > e.TotalX)
                        _game.Swipe(Direction.Top);
                    else
                        _game.Swipe(Direction.Right);
                }
                else
                {
                    if (e.TotalY > Math.Abs(e.TotalX))
                        _game.Swipe(Direction.Bottom);
                    else if (Math.Abs(e.TotalY) > Math.Abs(e.TotalX))
                        _game.Swipe(Direction.Top);
                    else
                        _game.Swipe(Direction.Left);
                }
            }
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            _panJustBegun = true;
        }
    }
}
