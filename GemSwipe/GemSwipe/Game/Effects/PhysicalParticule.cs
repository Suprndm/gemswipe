using System;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Effects
{
    public class PhysicalParticule : SkiaView
    {
        private float _g, _k, _a, _f, _vx, _vy;

        public PhysicalParticule(float g, float k, float a, float f, SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _vx = 0;
            _vy = 0;
            _g = g;
            _k = k;
            _a = a;
            _f = f;

            _vx = _vx + (float)Math.Cos(_a * Math.PI / 180) * _f;
            _vy = _vy + (float)Math.Sin(_a * Math.PI / 180) * _f + _g;
        }

        protected override void Draw()
        {
            var oldx = X;
            var oldy = Y;


            _vx = _vx * _k;
            _vy = _vy * _k;
            _x += _vx;
            _y += _vy;


            if(Math.Abs(_vx) + Math.Abs(_vy) <1)
            {
                Dispose();
            }

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.StrokeWidth = 2;
                paint.Color = CreateColor(255, 255, 255, 255);
                Canvas.DrawLine(oldx,oldy, X, Y, paint);
            }
        }
    }
}
