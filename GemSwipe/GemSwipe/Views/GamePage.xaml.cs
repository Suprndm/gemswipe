using System;
using System.Collections.Generic;
using System.Diagnostics;
using GemSwipe.Data.PlayerData;
using GemSwipe.Game.Models;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Gestures;
using GemSwipe.Services;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;
using Xamarin.Forms;

namespace GemSwipe.Views
{
    public partial class GamePage : ContentPage
    {
        bool pageIsActive;
        private bool _isInitiated;
        private SkiaRoot _skiaRoot;
        private SKCanvas _canvas;
        private Stopwatch _stopwatch;
        private long _lastElapsedTime = 0;
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

                if (_stopwatch.ElapsedMilliseconds - _lastElapsedTime > 0)
                {
                    var fps = 1000 / (_stopwatch.ElapsedMilliseconds - _lastElapsedTime);
                    _lastElapsedTime = _stopwatch.ElapsedMilliseconds;

                    _skiaRoot.UpdateFps(fps);
                }

            }
            else
            {
                // Init SkiaSharp
                _canvas = e.Surface.Canvas;
                _skiaRoot = new SkiaRoot(0, 0, e.Surface.Canvas.ClipBounds.Height, e.Surface.Canvas.ClipBounds.Width);
                _skiaRoot.Initialize();
                _skiaRoot.SetCanvas(e.Surface.Canvas);
                _isInitiated = true;
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
            }
        }

        #endregion

        #region Setup

        protected override void OnAppearing()
        {
            var fileHandler = DependencyService.Get<IFileHandler>();
            if (!(fileHandler.CheckExistenceOf(AppSettings.PlayerPersonalDataFileName)))
            {
                GetPlayerNickname();
            }

            SetupSkiaView();

            base.OnAppearing();
        }

        public async void GetPlayerNickname()
        {
            string myinput = await InputDialog.InputBox(this.Navigation);
            var playerData = PlayerDataService.Instance.GetData();
            playerData.Nickname = myinput;
            PlayerDataService.Instance.SaveChanges();
        }

        private void SetupSkiaView()
        {
            SKGLView.PaintSurface += SKGLView_PaintSurface;
        }

        #endregion

        private void Dispose()
        {
            _skiaRoot.Dispose();
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