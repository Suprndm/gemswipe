﻿using System;
using System.Collections.Generic;
using GemSwipe.GameEngine;
using GemSwipe.Models;
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
        private Game _game;
        private SKCanvas _canvas;

        public GamePage()
        {
            InitializeComponent();
        }

        #region Swipe

        private void Swipe(Direction direction)
        {
            _game.Swipe(direction);
        }

        #endregion


        #region Render


        private void SKGLView_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (_isInitiated)
            {
                e.Surface.Canvas.Clear(SKColors.Black);
                _game.Render();
            }
            else
            {
                // Init SkiaSharp
                _canvas = e.Surface.Canvas;
                _game = new Game(e.Surface.Canvas, 0, 0, e.Surface.Canvas.ClipBounds.Height, e.Surface.Canvas.ClipBounds.Width);
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

            Gestures.Gesture.SetTapped(SKGLView, new Command<Point>(OnCanvasTapped));
        }

        private void OnCanvasTapped(Point p)
        {
         
            _game.DetectTap(p);;
            _panJustBegun = true;
        }

        #endregion

        #region UserControls

        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.TotalX == 0 && e.TotalY == 0) return;

            var eX = e.TotalX;
            var eY = e.TotalY;
            var d = Math.Sqrt(eX * eX + eY * eY);

            if (d > 25 && !_game.IsBusy() && _panJustBegun)
            {
                _panJustBegun = false;
                if (eX > 0)
                {
                    if (eY > eX)
                        Swipe(Direction.Bottom);
                    else if (Math.Abs(eY) > eX)
                        Swipe(Direction.Top);
                    else
                        Swipe(Direction.Right);
                }
                else
                {
                    if (eY > Math.Abs(eX))
                        Swipe(Direction.Bottom);
                    else if (Math.Abs(eY) > Math.Abs(eX))
                        Swipe(Direction.Top);
                    else
                        Swipe(Direction.Left);
                }
            }
        }

        protected override void OnDisappearing()
        {
            SKGLView.PaintSurface -= SKGLView_PaintSurface;
        }

        #endregion

        private void Dispose()
        {
            _game.Dispose();
        }
    }
}