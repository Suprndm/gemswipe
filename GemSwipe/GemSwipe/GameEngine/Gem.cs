using System;
using System.Threading.Tasks;
using GemSwipe.GameEngine.SkiaEngine;
using GemSwipe.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace GemSwipe.GameEngine
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

        public Gem(int boardX, int boardY, int size) : base(null, 0, 0, 0, 0)
        {
            Size = size;
            BoardX = boardX;
            BoardY = boardY;
        }
        public Gem(int boardX, int boardY, int size, SKCanvas canvas, float x, float y, float radius) : base(canvas, x, y, radius * 2, radius * 2)
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
            _opacity = 1;

            DeclareTappable(this);


            Tapped += () =>
            {
            };
        }

        public void LevelUp()
        {
            _willLevelUp = true;
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


            var innerRadius = _radius * (2 + (_fluidSize / 2)) / 10;
            _cycle += (int)_fluidSize;
            _cycle = _cycle % 365;
            var result = (byte)(((Math.Cos(_cycle * Math.PI / 180) + 1) * 75 + 55) * _opacity);
            var colors = new SKColor[] {
                new SKColor (255, 255,255,result),
                new SKColor (255, 255, 255,0),

            };

            var shader = SKShader.CreateRadialGradient(new SKPoint(X + _radius, Y + _radius), innerRadius * 5f, colors, new[] { 0.0f, 1f }, SKShaderTileMode.Clamp);
            var glowPaint = new SKPaint()
            {
                Shader = shader,
            };
            Canvas.DrawCircle(X + _radius, Y + _radius, innerRadius * 5f, glowPaint);


            var innerPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = new SKColor(255, 255, 255, (byte)(255 * _opacity))
            };



            Canvas.DrawCircle(X + _radius, Y + _radius, innerRadius, innerPaint);



            for (int i = 0; i < Size; i++)
            {
                float lastRingOpacity = 1;
                if (i == Size - 1 && _fluidSize < Size)
                {
                    lastRingOpacity = 1 - (Size - _fluidSize);
                }
                using (var paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    paint.StrokeWidth = 4;
                    paint.Style = SKPaintStyle.Stroke;
                    paint.Color = new SKColor(255, 255, 255, (byte)(255 * _opacity * lastRingOpacity));

                    Canvas.DrawCircle(X + _radius, Y + _radius, (float)(innerRadius * (2 + (double)i / 3)), paint);
                }
            }
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
                this.Animate("fade", p => _opacity = (float) p, 1, 0, 4, MovementAnimationMs / 2, Easing.CubicOut);
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
                this.Animate("size", p => _fluidSize = (float) p, oldSize, _size, 4, MovementAnimationMs,
                    Easing.CubicOut);
            }
        }
    }
}
