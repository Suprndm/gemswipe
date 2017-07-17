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
            _fluidX = X;
            _fluidY = Y;
            _fluidSize = Width;
        }

        //public void Draw()
        //{
        //    var levelVariation = (float)((Gem.Size - _fluidLevel) * 0.2);
        //    _fluidLevel += levelVariation;
        //    if (Math.Abs(Gem.Size - _fluidLevel) < 0.01)
        //    {
        //        _fluidLevel = Gem.Size;
        //    }

        //    _fluidX += (float)((X - _fluidX) * 0.2);
        //    if (Math.Abs(X - _fluidX) < 1)
        //    {
        //        _fluidX = X;
        //    }

        //    _fluidY += (float)((Y - _fluidY) * 0.2);
        //    if (Math.Abs(Y - _fluidY) < 1)
        //    {
        //        _fluidY = Y;
        //    }

        //    var sizeVariation = Math.Sin(levelVariation * 4 * Math.PI);
        //    _fluidSize = (float)(Width + sizeVariation * (Width / 3));

        //    if (Gem.IsDead())
        //    {
        //        Die();
        //        _isDying = true;
        //    }
        //}

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

            Canvas.DrawCircle(_fluidX, _fluidY, _fluidSize, gemColor);
            Canvas.DrawCircle(_fluidX, _fluidY - (_fluidSize - _fluidSize * 7 / 10), _fluidSize * 7 / 10, gemReflectColor);
        }

        public  void MoveTo(float x, float y)
        {
            var oldX = X;
            var oldY = Y;

            X = x;
            Y = y;

            this.Animate("moveX", p => this._fluidX = (float)p, oldX, X, 4, MovementAnimationMs, Easing.CubicOut);
            this.Animate("moveY", p => this._fluidY = (float)p, oldY, Y, 8, MovementAnimationMs, Easing.CubicOut);


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
