﻿using System;
using System.Threading.Tasks;
using GemSwipe.Game.Effects;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Entities
{
    public class Gem : SkiaView
    {
        public int BoardX { get; private set; }
        public int BoardY { get; private set; }
        public int Size { get; private set; }
        public int TargetBoardX { get; private set; }
        public int TargetBoardY { get; private set; }
        private bool _willLevelUp;
        private bool _willDie;
        private bool _isDead;
        private float _fluidX;
        private float _fluidY;
        private float _fluidLevel;
        private float _fluidSize;
        private int _size;
        private bool _isDying;
        private readonly float _radius;
        private const int MovementAnimationMs = 600;
        private Random _randomizer;
        private int _cycle;
        private int _cycleSpeed;
        private float _opacity;
        private float _angle;

        private SKColor _finalColor;

        private bool _isActive;

        public Gem(int boardX, int boardY, int size) : base(0, 0, 0, 0)
        {
            Size = size;
            BoardX = boardX;
            BoardY = boardY;
        }
        public Gem(int boardX, int boardY, int size, float x, float y, float radius) : base(x, y, radius * 2, radius * 2)
        {
            _randomizer = new Random();
            _cycle = 0;
            Size = size;
            _fluidSize = size;
            BoardX = boardX;
            BoardY = boardY;
            _radius = radius;
            _size = size;
            _fluidX = _x;
            _fluidY = _y;
            _opacity = 0;
            _angle = (float)(_randomizer.Next(100) * Math.PI * 2 / 100);
            _finalColor = new SKColor(195, 184, 85);

        }

        public void LevelUp()
        {
            _willLevelUp = true;
        }

        private void Shine()
        {
            var effect = new GemPopEffect(Width / 2, Height / 2, Height, Width);
            AddChild(effect);
            effect.Start();
        }
        public async Task Pop()
        {
            Shine();
            await Task.Delay(150);
            this.Animate("opacity", p => _opacity = (float)p, 0, 1, 4, 320, Easing.CubicOut);
        }

        public void Die()
        {
            _willDie = true;
        }

        public bool CanMerge()
        {
            return !_willLevelUp && !_willDie;
        }

        public void Resolve()
        {
            if (_willLevelUp)
            {
                Size++;
                _willLevelUp = false;
            }

            BoardX = TargetBoardX;
            BoardY = TargetBoardY;

            _isDead = _willDie;
        }


        public bool IsDead()
        {
            return _isDead;
        }

        public bool WillDie()
        {
            return _willDie;
        }

        public void Move(int x, int y)
        {
            TargetBoardX = x;
            TargetBoardY = y;
        }

        protected override void Draw()
        {
            var starX = X + _radius;
            var starY = Y + _radius;

            var branches = (int)_fluidSize + 1;
            var reduction = 1f / (8f / Math.Min(8, branches));
            var starRadius = (float)(_radius * Math.Min(1, reduction + 0.3));

            var colorReduction = reduction * reduction;
            var starRed = (byte)(255 - (255 - _finalColor.Red) * colorReduction);
            var starGreen = (byte)(255 - (255 - _finalColor.Green) * colorReduction);
            var starBlue = (byte)(255 - (255 - _finalColor.Blue) * colorReduction);
            var startColor = CreateColor(starRed, starGreen, starBlue, (byte)(255 * _opacity));

            _angle += 0.001f / (_size * 0.5f);
            var outerRadius = starRadius * 1.0f;
            var innerRadius = outerRadius * .4f;

            var colors = new SKColor[] {
                CreateColor (255, 255,255, (byte)(200*_opacity*(reduction))),
                CreateColor (255, 255, 255,0),
            };

            var shader = SKShader.CreateRadialGradient(new SKPoint(X + _radius, Y + _radius), starRadius * 1.5f, colors, new[] { 0.0f, 1f }, SKShaderTileMode.Clamp);
            var glowPaint = new SKPaint()
            {
                Shader = shader
            };

            Canvas.DrawCircle(starX, starY, starRadius * 1.5f, glowPaint);

            // Shadowed


            float reductionCoef = 0.95f;


            var points = Polygonal.GetStarPolygon(innerRadius * reductionCoef, outerRadius * reductionCoef, branches,
                2 * (float)(_angle + Math.PI / 2 * 1 / branches));

            var path = new SKPath();

            for (int k = 0; k < points.Count; k++)
            {
                var point = points[k];
                var translatedPoint = new SKPoint(point.X + starX, point.Y + starY);
                if (k == 0)
                    path.MoveTo(translatedPoint);
                else
                    path.LineTo(translatedPoint);
            }

            path.Close();

            var paint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = CreateColor(175, 175, 175, (byte)(255 * _opacity)),
                StrokeWidth = 2,
                IsAntialias = false
            };

            Canvas.DrawPath(path, paint);

            points = Polygonal.GetStarPolygon(innerRadius, outerRadius, branches, 2 * _angle);
            path = new SKPath();
            for (int k = 0; k < points.Count; k++)
            {
                var point = points[k];
                var translatedPoint = new SKPoint(point.X + starX, point.Y + starY);
                if (k == 0)
                    path.MoveTo(translatedPoint);
                else
                    path.LineTo(translatedPoint);
            }

            path.Close();

            paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = CreateColor(255, 255, 255, (byte)(255 * _opacity)),
                IsAntialias = true
            };

            Canvas.DrawPath(path, paint);

            paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = startColor,
                StrokeWidth = 10*_radius/100,
                IsAntialias = true
            };

            Canvas.DrawPath(path, paint);

            paint.StrokeWidth = 2;
            paint.Style = SKPaintStyle.Stroke;
            Canvas.DrawCircle(starX, starY, outerRadius * 1.2f, paint);
        }

        public void MoveTo(float x, float y)
        {
            var oldX = _x;
            var oldY = _y;

            var newX = x;
            var newY = y;
            if (Canvas != null)
            {
                this.Animate("moveX", p => _x = (float)p, oldX, newX, 4, MovementAnimationMs, Easing.CubicOut);
                this.Animate("moveY", p => _y = (float)p, oldY, newY, 8, MovementAnimationMs, Easing.CubicOut);
            }

        }

        public async void DieTo(float x, float y)
        {
            MoveTo(x, y);
            if (Canvas != null)
            {
                await Task.Delay(MovementAnimationMs / 2);
                this.Animate("fade", p => _opacity = (float)p, 1, 0, 4, MovementAnimationMs / 2, Easing.CubicOut);
                await Task.Delay(MovementAnimationMs / 2);
            }
            Dispose();
        }

        public async void Fuse()
        {
            var oldSize = _size;
            _size++;
            if (Canvas != null)
            {
                await Task.Delay(MovementAnimationMs / 2);
                this.Animate("size", p => _fluidSize = (float)p, oldSize, _size, 4, MovementAnimationMs,
                    Easing.CubicOut);
                Shine();
            }
        }
    }
}
