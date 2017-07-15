using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GemSwipe.Views
{
    public partial class GamePage : ContentPage
    {
        private Game _game;
        private Stopwatch _stopwatch;
        private SKCanvas _canvas;
        bool pageIsActive;
        public GamePage()
        {
            InitializeComponent();

            _game = new Game(5, 5);
            _game.InitGame();
            _stopwatch = new Stopwatch();
        }

        private void Left(object sender, EventArgs e)
        {
            _game.Swipe(Direction.Left);
        }

        private void Right(object sender, EventArgs e)
        {
            _game.Swipe(Direction.Right);
        }

        private void Top(object sender, EventArgs e)
        {
            _game.Swipe(Direction.Top);
        }

        private void Bottom(object sender, EventArgs e)
        {
            _game.Swipe(Direction.Bottom);
        }

        //private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        //{
        //    var surface = e.Surface;
        //    _canvas = surface.Canvas;
        //    DrawBoard(_canvas, _game.GetBoard());
        //}

        private void OnCanvasViewPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            var surface = e.Surface;
            _canvas = surface.Canvas;
            DrawBoard(_canvas, _game.GetBoard());
            
        }   
        private void DrawBoard(SKCanvas canvas, Board board)
        {
            canvas.Clear(SKColors.White);
            var cellHeight = 180;
            var cellWidth = 180;


            // create the paint for the filled circle
            var cellColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(0, 0, 93)
            };

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    canvas.DrawRect(SKRect.Create(i * (cellWidth + 10) + 50, (cellHeight + 10) * j + 50, cellWidth, cellHeight), cellColor);
                }
            }

            var gems = board.GetGems().Select(g => g).ToList();
            Title = board.GetGems().Count.ToString();

            foreach (var gem in gems.Where(gem => gem.WillDie()))
            {
                DrawGem(gem);
            }

            foreach (var gem in gems.Where(gem => !gem.WillDie()))
            {
                DrawGem(gem);
            }

            foreach (var gem in gems)
            {
                gem.UpdatePosition();
            }


        }

        private void DrawGem(Gem gem)
        {
            var cellHeight = 180;
            var cellWidth = 180;
            var gemColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(330-gem.Size*10, 100, 50)
            };


            var textColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(0, 0, 100),
                TextSize = (int)(35 * (1 + (double)gem.Size / 5))
            };

            var gemWidth = (int)(35 * (1 + (double)gem.Size / 5));

            _canvas.DrawCircle(gem.FluidX * (cellWidth + 10) + 50 + cellWidth / 2, (cellHeight + 10) * gem.FluidY + 50 + cellWidth / 2, gemWidth, gemColor);
            _canvas.DrawText(gem.Size.ToString(), gem.FluidX * (cellWidth + 10) + 50 + cellWidth / 2, (cellHeight + 10) * gem.FluidY + 50 + cellWidth / 2 + gemWidth / 2, textColor);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageIsActive = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageIsActive = false;
        }

        async Task AnimationLoop()
        {
            while (pageIsActive)
            {
             

                await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
                canvasView.InvalidateSurface();
            }

            //while (true)
            //{

            //}
        }

        private void OnCanvasViewTapped(object sender, EventArgs e)
        {
        }


        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var d = Math.Sqrt(e.TotalX * e.TotalX + e.TotalY * e.TotalY);
            if (d > 100)
            {
                _game.Swipe(Direction.Bottom);
            }
        }
    }
}
