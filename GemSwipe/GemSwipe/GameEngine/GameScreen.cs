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
    public class GameScreen : SKGLView
    {
        private IGameState _game;
        private BoardGLView _boardGlView;
        private bool _isInitiated;

        public void InitDisplay(IGameState game)
        {
            _game = game;
            this.PaintSurface += GameScreen_PaintSurface;
        }


        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent != null)
                this.InvalidateSurface();
        }

        private void GameScreen_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (_isInitiated)
            {
                e.Surface.Canvas.Clear(SKColors.White);
                _boardGlView.Refresh(e.Surface.Canvas);
            }
            else
            {
                _isInitiated = true;
                _boardGlView = new BoardGLView(_game.GetBoardState(), 0, 0, (float)e.Surface.Canvas.ClipBounds.Width, (float)e.Surface.Canvas.ClipBounds.Width);
            }

        }

        public void EndDisplay()
        {
            PaintSurface -= GameScreen_PaintSurface;
        }
    }
}