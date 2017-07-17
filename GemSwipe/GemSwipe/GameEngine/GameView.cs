using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace GemSwipe.GameEngine
{
    public class GameView : SKGLView
    {
        private BoardView _boardView;
        private bool _isInitiated;
        private GameSetup _gameSetup;

        public void Setup(GameSetup gameSetup)
        {
            PaintSurface += GameScreen_PaintSurface;
            _gameSetup = gameSetup;
        }

        /// <summary>
        /// GameLoop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameScreen_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (_isInitiated)
            {
                e.Surface.Canvas.Clear(SKColors.White);
                _boardView.Render();
            }
            else
            {
                _isInitiated = true;
                _boardView = new BoardView(e.Surface.Canvas, 0, 0, (float)e.Surface.Canvas.ClipBounds.Width, (float)e.Surface.Canvas.ClipBounds.Width);
                _boardView.Setup(_gameSetup.Columns,_gameSetup.Rows);
                _boardView.Populate(_gameSetup.Gems);
            }
        }

        public void Update(GameUpdate gameUpdate)
        {
            _boardView.Update(gameUpdate);
        }

        public void EndDisplay()
        {
            PaintSurface -= GameScreen_PaintSurface;
        }
    }
}