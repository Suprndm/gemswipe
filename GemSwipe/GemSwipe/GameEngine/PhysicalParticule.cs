using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace GemSwipe.GameEngine
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
        }

        protected override void Draw()
        {
            _vx = _vx + (float)Math.Cos(_a * Math.PI / 360) * _f;
            _vy = _vy + (float)Math.Sin(_a * Math.PI / 360) * _f + _g;

            _x += _vx;
            _y += _vy;

            _f = _f * _k;

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(255, 255, 255, 255);
                Canvas.DrawCircle(X, Y, Height, paint);
            }
        }
    }
}
