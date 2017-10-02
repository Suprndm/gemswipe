using System;
using System.Collections.Generic;
using GemSwipe.Game.Gestures;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace GemSwipe.Views
{
    public partial class TestPage : ContentPage
    {
        private bool _panJustBegun;
        bool pageIsActive;
        private bool _isInitiated;
        private SKCanvas _canvas;

        public TestPage()
        {
            InitializeComponent();
        }

   
        #region Render


        private void SKGLView_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (_isInitiated)
            {
                e.Surface.Canvas.Clear(SKColors.Black);
            }
            else
            {
                // Init SkiaSharp
                _canvas = e.Surface.Canvas;
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

            Gesture.SetTapped(SKGLView, new Command<Point>(OnCanvasTapped));
        }

        private void OnCanvasTapped(Point p)
        {
         
            _panJustBegun = true;
        }

   

        #endregion

        #region UserControls

        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
           
        }

        protected override void OnDisappearing()
        {
            SKGLView.PaintSurface -= SKGLView_PaintSurface;
        }

        #endregion

        private void Dispose()
        {
        }
    }
}