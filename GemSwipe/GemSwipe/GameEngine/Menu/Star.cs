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
        public float Z { get; set; }
        public double Speed { get; set; }
        public double Phase { get; set; }
        private double _angle;
        private float _size;
        private float _direction { get; }

        private float _targetY;
        private Random _randomizer;
        public Star(SKCanvas canvas, float x, float y, float height, float width, float z, double speed, double phase) : base(canvas, x, y, height, width)
        {
            _direction = 1;
            _targetY = Y;
            Z = z;
            _angle = phase;
            Speed = speed;
            Phase = phase;
            _size = (7 - Z);
        }

        public Star(SKCanvas canvas, Random randomizer, float height, float width) : base(canvas, 0, 0, height, width)
        {
            _direction = -1;
            _randomizer = randomizer;
            _y = _randomizer.Next((int)Height);

            ResetRandomCinematicProperties();
        }

        public void ResetRandomCinematicProperties()
        {
            _x = _randomizer.Next((int)Width);
            Z = _randomizer.Next(1, 7);
            Speed = _randomizer.Next(10) / 100f;
            Phase = _randomizer.Next(400) / 100;

            _targetY = Y;
            _angle = Phase;
            _size = (7 - Z);
        }

        protected override void Draw()
        {
            _targetY += _direction*Height / 10000 * _size;
            if (Math.Abs(_targetY - Y) < 2)
            {
                Y = _targetY;
            }
            else
            {
                _y += (_targetY - Y) * 0.04f;
            }

            //if (_y < 0)
            //{
            //    _y = Height;
            //    _targetY = _y + _targetY;
            //    ResetRandomCinematicProperties();
            //}

            if (_y > Height)
            {
                _y = 0;
                _targetY = _y + _targetY;
                ResetRandomCinematicProperties();
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

            //using (var secondPaint = new SKPaint())
            //{
            //    secondPaint.IsAntialias = false;
            //    secondPaint.Style = SKPaintStyle.Stroke;
            //    secondPaint.StrokeWidth = 2 * _size / 3;
            //    secondPaint.Color = new SKColor(255, 255, 255, (byte)(opacity * 255));
            //    Canvas.DrawPoint(X, Y, secondPaint);
            //}
        }

        public void Slide()
        {
            _targetY += -Height / 40 * _size;
        }
    }
}
