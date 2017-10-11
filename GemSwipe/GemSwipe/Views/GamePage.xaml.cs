using System;
using System.Collections.Generic;
using GemSwipe.Game.Entities;
using GemSwipe.Game.Gestures;
using GemSwipe.Game.Models;
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
        private SkiaRoot _skiaRoot;
        private SKCanvas _canvas;

        public GamePage()
        {
            InitializeComponent();
        }

        #region Render

        private void SKGLView_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (_isInitiated)
            {
                e.Surface.Canvas.Clear(SKColors.Black);
                _skiaRoot.Render();
            }
            else
            {
                // Init SkiaSharp
                _canvas = e.Surface.Canvas;
                _skiaRoot = new SkiaRoot( 0, 0, e.Surface.Canvas.ClipBounds.Height, e.Surface.Canvas.ClipBounds.Width);
                _skiaRoot.SetCanvas(e.Surface.Canvas);
                _isInitiated = true;
            }
        }

        #endregion

        #region Setup

        protected override void OnAppearing()
        {
            SetupSkiaView();

            base.OnAppearing();
        }


        private void SetupSkiaView()
        {
            _panJustBegun = true;
            SKGLView.PaintSurface += SKGLView_PaintSurface;


            Gesture.Setup(SKGLView);
        }

        #endregion

        private void Dispose()
        {
            _skiaRoot.Dispose();
        }
    }
}