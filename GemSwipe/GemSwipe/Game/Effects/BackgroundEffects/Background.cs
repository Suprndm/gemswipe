﻿using System;
using System.Threading.Tasks;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class Background : SkiaView
    {
        private readonly OceanDepth _oceanDepth;
        private readonly Stars _stars;

        public Background(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height,
            width)
        {
            _oceanDepth = new OceanDepth(Canvas, X, Y, Height, Width);
            AddChild(_oceanDepth);

            var blackHalo = new Halo(Canvas, X - Width / 3, Y, Height, Width * 3f, new SKColor(255, 255, 255), Math.PI);
            var whiteHalo = new Halo(Canvas, X - Width / 3, Y, Height, Width * 3f, new SKColor(0, 0, 0), 0);
            AddChild(blackHalo);
            AddChild(whiteHalo);

            _stars = new Stars(Canvas, X, Y, Height, Width);
            AddChild(_stars);
        }

        public Task OnNextBoard()
        {
            _stars.ScrollDown();
            return _oceanDepth.ScrollDown();
        }

        public Task PlayTransition(PageType currentPage, PageType nextPage)
        {
            return Task.Delay(1000);
        }

        protected override void Draw()
        {

        }
    }
}