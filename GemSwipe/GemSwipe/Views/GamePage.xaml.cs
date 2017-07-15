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
        private bool _panJustBegun;
        private Game _game;
        private Stopwatch _stopwatch;
        private SKCanvas _canvas;
        bool pageIsActive;
        public GamePage()
        {
            InitializeComponent();

            _game = new Game(5, 8);
            _game.InitGame();
            _stopwatch = new Stopwatch();
            _panJustBegun = true;
        }

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
                Color = SKColor.FromHsl(330-gem.Size*20, 100, 50)
            };

            var gemLightColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(330 - gem.Size * 20, 90, 60)
            };


            var gemReflectColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(330 - gem.Size * 20, 90, 65)
            };


            var textColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(0, 0, 100),
                TextSize = (int)(35 * (1 + (double)gem.Size / 5))
            };

            var gemWidth = (int)(35 * (1 + (double)gem.Size / 7));

            _canvas.DrawCircle(gem.FluidX * (cellWidth + 10) + 50 + cellWidth / 2, (cellHeight + 10) * gem.FluidY + 50 + cellWidth / 2, gemWidth, gemColor);
            _canvas.DrawCircle(gem.FluidX * (cellWidth + 10) + 50 + cellWidth / 2, (cellHeight + 10) * gem.FluidY + 50 + cellWidth / 2- (gemWidth-gemWidth * 7 / 10), gemWidth* 7 / 10, gemReflectColor);
            //_canvas.DrawText(gem.Size.ToString(), gem.FluidX * (cellWidth + 10) + 50 + cellWidth / 2, (cellHeight + 10) * gem.FluidY + 50 + cellWidth / 2 + gemWidth / 2, textColor);
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
