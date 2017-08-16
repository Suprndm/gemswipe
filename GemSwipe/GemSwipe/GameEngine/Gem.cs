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
        private const int MovementAnimationMs = 300;

        public Gem(int boardX, int boardY, int size) : base(null, 0, 0, 0, 0)
        {
            Size = size;
            BoardX = boardX;
            BoardY = boardY;
        }
        public Gem(int boardX, int boardY, int size, SKCanvas canvas, float x, float y, float radius) : base(canvas, x, y, radius * 2, radius * 2)
        {
            Size = size;
            BoardX = boardX;
            BoardY = boardY;
            _radius = radius;
            _size = size;
            _fluidX = _x;
            _fluidY = _y;

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
            var gemColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(330 - _size * 20, 100, 50)
            };

            var gemReflectColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(330 - _size * 20, 90, 65)
            };

            //Glow 
            var colors = new SKColor[] {
                SKColor.FromHsl(330 - _size * 20, 100, 50),
                SKColor.FromHsl(330 - _size * 20, 100, 50,0)
            };
            var shader = SKShader.CreateRadialGradient(new SKPoint(X + _radius, Y+ _radius), _fluidSize * 1.3f, colors, new[] { 0.5f, 1f }, SKShaderTileMode.Clamp);


            _fluidSize = _radius;
            var paint = new SKPaint()
            {
                Shader = shader,
            };

            Canvas.DrawCircle(X + _radius, Y + _radius, _fluidSize * 19 / 10, paint);
            Canvas.DrawCircle(X + _radius, Y + _radius, _fluidSize, gemColor);
            Canvas.DrawCircle(X + _radius, Y + _radius - (_fluidSize - _fluidSize * 7 / 10), _fluidSize * 7 / 10, gemReflectColor);
        }

        public void MoveTo(float x, float y)
        {
            var oldX = _x;
            var oldY = _y;

            var newX = x;
            var newY = y;

            this.Animate("moveX", p => _x = (float)p, oldX, newX, 4, MovementAnimationMs, Easing.CubicOut);
            this.Animate("moveY", p => _y = (float)p, oldY, newY, 8, MovementAnimationMs, Easing.CubicOut);
        }

        public async void DieTo(float x, float y)
        {
            MoveTo(x, y);
            await Task.Delay(MovementAnimationMs);
            Dispose();
        }

        public void Fuse()
        {
            _size++;
        }
    }
}
