using System;
using System.Threading.Tasks;
using GemSwipe.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace GemSwipe.GameEngine
{
    public class GemGLView : GLViewBase
    {
        public Guid Id { get; }
        public IGemState GemState;
        public event Action<GemGLView> Disposed;
        private float _fluidX;
        private float _fluidY;
        private float _fluidLevel;
        private float _fluidSize;
        private int _size;
        private bool _isDying;

        public GemGLView(IGemState gemState, float x, float y, float height, float width) : base(x, y, height, width)
        {
            GemState = gemState;
            Id = GemState.Id;
            _size = gemState.Size;
        }

        public override void UpdateState()
        {
            var levelVariation = (float)((GemState.Size - _fluidLevel) * 0.2);
            _fluidLevel += levelVariation;
            if (Math.Abs(GemState.Size - _fluidLevel) < 0.01)
            {
                _fluidLevel = GemState.Size;
            }

            _fluidX += (float)((X - _fluidX) * 0.2);
            if (Math.Abs(X - _fluidX) < 1)
            {
                _fluidX = X;
            }

            _fluidY += (float)((Y - _fluidY) * 0.2);
            if (Math.Abs(Y - _fluidY) < 1)
            {
                _fluidY = Y;
            }

            var sizeVariation = Math.Sin(levelVariation * 4 * Math.PI);
            _fluidSize = (float)(Width + sizeVariation * (Width / 3));

            if (GemState.IsDead())
            {
                Die();
                _isDying = true;
            }
        }

        public override void Draw(SKCanvas canvas)
        {
            var gemColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(330 - GemState.Size * 20, 100, 50)
            };

            var gemReflectColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(330 - GemState.Size * 20, 90, 65)
            };

            canvas.DrawCircle(_fluidX, _fluidY, _fluidSize, gemColor);
            canvas.DrawCircle(_fluidX, _fluidY - (_fluidSize - _fluidSize * 7 / 10), _fluidSize * 7 / 10, gemReflectColor);
        }

        public override void MoveTo(float x, float y)
        {
            X = x;
            Y = y;
        }

        private async void Die()
        {
            await Task.Delay(300);

            Disposed?.Invoke(this);

            Dispose();
        }

        public bool IsDying()
        {
            return _isDying;
        }

        public override void Dispose()
        {
        }
    }
}
