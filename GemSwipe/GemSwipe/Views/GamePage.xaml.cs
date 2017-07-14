using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GemSwipe.Views
{
    public partial class GamePage : ContentPage
    {
        private Game _game;
        public GamePage()
        {
            InitializeComponent();

            _game = new Game(5, 5);
            _game.InitGame();
            DrawBoard();
        }

        private void Left(object sender, EventArgs e)
        {
            _game.Swipe(Direction.Left);
            DrawBoard();
        }

        private void Right(object sender, EventArgs e)
        {
            _game.Swipe(Direction.Right);
            DrawBoard();
        }

        private void Top(object sender, EventArgs e)
        {
            _game.Swipe(Direction.Top);
            DrawBoard();
        }

        private void Bottom(object sender, EventArgs e)
        {
            _game.Swipe(Direction.Bottom);
            DrawBoard();
        }



        public void DrawBoard()
        {
            var board = _game.GetBoard();

            var cellsGrid = board.GetCells();
            string draw = "";
            for (int j = 0; j < board.Height; j++)
            {
                for (int i = 0; i < board.Width; i++)
                {
                    draw += " ";
                    var cell = cellsGrid[i, j];
                    var gem = cell.GetAttachedGem();
                    if (gem == null)
                        draw += "0";
                    else draw += gem.Size;
                    draw += " ";
                }
                draw += "\n";
            }

            BoardDisplay.Text = draw;
        }

        //private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        //{
        //    //// we get the current surface from the event args
        //    //var surface = e.Surface;
        //    //// then we get the canvas that we can draw on
        //    //var canvas = surface.Canvas;
        //    //// clear the canvas / view
        //    //canvas.Clear(SKColors.White);

        //    //// create the paint for the filled circle
        //    //var circleFill = new SKPaint
        //    //{
        //    //    IsAntialias = true,
        //    //    Style = SKPaintStyle.Fill,
        //    //    Color = SKColors.Blue
        //    //};

        //    //// draw the circle fill
        //    //canvas.DrawCircle(100, 100, 40, circleFill);
        //}
    }
}
