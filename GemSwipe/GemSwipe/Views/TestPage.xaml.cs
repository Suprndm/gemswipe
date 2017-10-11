using System;
using System.Collections.Generic;
using System.Diagnostics;
using GemSwipe.Game;
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
        private TestView _testView;
        private Stopwatch _stopwatch;
        private long _lastElapsedTime = 0;
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
                _testView.Render();

                var fps = 1000/(_stopwatch.ElapsedMilliseconds - _lastElapsedTime);
                _lastElapsedTime = _stopwatch.ElapsedMilliseconds;

                _testView.UpdateFps(fps);
            }
            else
            {
                // Init SkiaSharp
                _canvas = e.Surface.Canvas;
                _testView = new TestView( 0, 0, e.Surface.Canvas.ClipBounds.Height, e.Surface.Canvas.ClipBounds.Width);
                _testView.SetCanvas(e.Surface.Canvas);
                _isInitiated = true;
                _stopwatch = new Stopwatch();
                _stopwatch.Start();

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