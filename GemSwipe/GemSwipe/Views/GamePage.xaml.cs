using System;
using System.Collections.Generic;
using GemSwipe.Data.PlayerData;
using GemSwipe.Game.Entities;
using GemSwipe.Game.Gestures;
using GemSwipe.Game.Models;
using GemSwipe.Services;
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
                _skiaRoot = new SkiaRoot(0, 0, e.Surface.Canvas.ClipBounds.Height, e.Surface.Canvas.ClipBounds.Width);
                _skiaRoot.SetCanvas(e.Surface.Canvas);
                _isInitiated = true;
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

            //PlayerDataService.Instance.Update(playerData);
            PlayerDataService.Instance.SaveChanges();
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