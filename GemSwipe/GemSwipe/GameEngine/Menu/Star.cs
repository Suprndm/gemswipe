using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.GameEngine.Menu
{
    public class Star : SkiaView
    {
        public float Z { get; }
        public double Speed { get; }
        public double Phase { get; }
        private double _angle;
        private float _size;

        private float _targetY;
        private Random _randomizer;
        public Star(SKCanvas canvas, float x, float y, float height, float width, float z, double speed, double phase) : base(canvas, x, y, height, width)
        {
            _targetY = Y;
            Z = z;
            _angle = phase;
            Speed = speed;
            Phase = phase;
            _size = (7 - Z);
        }

        protected override void Draw()
        {
            _targetY += -(float)Height / 10000 * _size;
            if (Math.Abs(_targetY - Y) < 2)
            {
                Y = _targetY;
            }
            else
            {
                _y += (_targetY - Y) * 0.04f;
            }

            if (_y < 0)
            {
                _y = Height;
                _targetY = _y + _targetY;
            }

            var opacity = (Math.Cos((_angle)) + 1) / 2;
            _angle += Speed;

            //var colors = new SKColor[] {
            //    new SKColor(255,255,255, (byte)( opacity*100)),
            //    new SKColor(255,255,255, 0),
            //};

            //var shader = SKShader.CreateRadialGradient(new SKPoint(X, Y), _size, colors, new[] { 0.0f, 1f }, SKShaderTileMode.Clamp);
            //var paint = new SKPaint() { Shader = shader };
            //Canvas.DrawCircle(X, Y, _size, paint);

            using (var secondPaint = new SKPaint())
            {
                secondPaint.IsAntialias = true;
                secondPaint.Style = SKPaintStyle.Fill;
                secondPaint.Color = new SKColor(255, 255, 255, (byte)(opacity * 255));
                Canvas.DrawCircle(X, Y, _size / 3, secondPaint);
            }
        }

        public void Slide()
        {
            _targetY += -Height / 40 * _size;
        }
    }
}
