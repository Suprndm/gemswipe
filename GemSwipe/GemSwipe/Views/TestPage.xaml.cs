using System;
using System.Diagnostics;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Test;
using GemSwipe.Paladin.Gestures;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;
using Xamarin.Forms;

namespace GemSwipe.Views
{
    public partial class TestPage : ContentPage
    {
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
                e.Surface.Canvas.Clear(new SKColor(0, 0, 0));
                _testView.Render();

                var fps = 1000 / (_stopwatch.ElapsedMilliseconds - _lastElapsedTime);
                _lastElapsedTime = _stopwatch.ElapsedMilliseconds;

                _testView.UpdateFps(fps);
            }
            else
            {
                // Init SkiaSharp
                _canvas = e.Surface.Canvas;
                _testView = new TestView(0, 0, e.Surface.Canvas.ClipBounds.Height, e.Surface.Canvas.ClipBounds.Width);
                _testView.SetCanvas(e.Surface.Canvas);
                _testView.Initialize();
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
            SKGLView.PaintSurface += SKGLView_PaintSurface;
        }

        #endregion

        #region UserControls


        protected override void OnDisappearing()
        {
            SKGLView.PaintSurface -= SKGLView_PaintSurface;
        }

        #endregion

        private void Dispose()
        {
        }


        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            var width = Layout.Width;
            var height = Layout.Height;
            var deviceHeight = SkiaRoot.ScreenHeight;
            var deviceWidth = SkiaRoot.ScreenWidth;
            var posX = Math.Max(0, args.Location.X / width * deviceWidth);
            var posY = Math.Max(0, args.Location.Y / height * deviceHeight);
            posX = Math.Min(posX, SkiaRoot.ScreenWidth * .95f);
            posY = Math.Min(posY, SkiaRoot.ScreenHeight * .95f);

            var motionPosition = new Point(posX, posY);

            switch (args.Type)
            {
                case TouchActionType.Entered:

                    break;
                case TouchActionType.Pressed:
                    Gesture.OnDown(motionPosition);
                    break;
                case TouchActionType.Moved:
                    Gesture.OnPan(motionPosition);
                    break;
                case TouchActionType.Released:
                    Gesture.OnUp(motionPosition);
                    break;
                case TouchActionType.Cancelled:

                    break;
                case TouchActionType.Exited:

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}