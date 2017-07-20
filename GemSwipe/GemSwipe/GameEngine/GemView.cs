using System;
using System.Threading.Tasks;
using GemSwipe.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace GemSwipe.GameEngine
{
    public class GemView : SkiaView
    {
        public Guid Id { get; }
        private float _fluidX;
        private float _fluidY;
        private float _fluidLevel;
        private float _fluidSize;
        private int _size;
        private bool _isDying;
        private const int MovementAnimationMs = 300;
        public GemView(Guid id, SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            Id = id;
            _size = 0;
            _fluidX = _x;
            _fluidY = _y;
            _fluidSize = Width;
        }

        protected override void Draw()
        {
            var gemColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(330 -_size * 20, 100, 50)
            };

            var gemReflectColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(330 - _size * 20, 90, 65)
            };

            Canvas.DrawCircle(X, Y, _fluidSize, gemColor);
            Canvas.DrawCircle(X, Y - (_fluidSize - _fluidSize * 7 / 10), _fluidSize * 7 / 10, gemReflectColor);
        }

        public  void MoveTo(float x, float y)
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
            _fluidSize = Width;
        }
    }
}
