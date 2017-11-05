using System;
using Android.Support.V4.App;
using GemSwipe.Paladin.Core;
using SkiaSharp;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class Star : SkiaView
    {
        private float _direction { get; }
        private Random _randomizer;

        private float _size;
        private float _depthZ { get; set; }
        private float _baseVelocity;
        private float _velocityY { get; set; }
        private float _accelerationY { get; set; }

        private double _phase { get; set; }
        private double _speed { get; set; }
        
        public float TargetVelocity { get; set; }
        private float _targetY;

        public Star( float x, float y, float height, float width, float z, double speed, double phase) : base( x, y, height, width)
        {
            _direction = 1;
            _targetY = Y;
            _depthZ = z;
            _speed = speed;
            _phase = phase;
            _size = (7 - _depthZ);
        }

        public Star( Random randomizer, float height, float width) : base( 0, 0, height, width)
        {
            _direction = 1;
            _randomizer = randomizer;

            _x = _randomizer.Next((int)Width);

            _y = _randomizer.Next((int)Height);
            _velocityY = _direction * _randomizer.Next((int)(5 * Height / 2000), (int)(8 * Height / 2000));
            _baseVelocity = _velocityY;
            TargetVelocity = _velocityY;

            ResetRandomCinematicProperties();
        }

        public void ResetRandomCinematicProperties()
        {
            _depthZ = _randomizer.Next(1, 14);
            _size = (14 - _depthZ);

            _phase = _randomizer.Next(400) / 100;
            _speed = _randomizer.Next(10) / 100f;
        }

        private void ApplyForce()
        {
            _targetY = _y + _direction * TargetVelocity;
            float desiredVelocity = _targetY - _y;
            float steering = desiredVelocity - _velocityY;
            _accelerationY = steering * 0.05f;
        }

        private void Update()
        {
           if (_y < 0)
            {
                _y = Height;
                ResetRandomCinematicProperties();
            }

            if (_y > Height)
            {
                _y = 0;
                ResetRandomCinematicProperties();
            }

            ApplyForce();
            _velocityY += _accelerationY;
            _y += _velocityY;

            _phase += _speed*0.2f;
            _opacity = (float)(Math.Cos(_phase) + 1) / 2;
            _accelerationY = 0;
        }

        protected override void Draw()
        {
            Update();


            var colors = new SKColor[] {
                CreateColor (255, 255,255, (byte)(25*_opacity)),
                CreateColor (255, 255, 255,0),
            };

            var glowSize = _size * _size;

            var shader = SKShader.CreateRadialGradient(new SKPoint(X, Y), glowSize, colors, new[] { 0.0f, 1f }, SKShaderTileMode.Clamp);
            var glowPaint = new SKPaint()
            {
                Shader = shader,
            };

            Canvas.DrawCircle(X, Y, glowSize, glowPaint);


            using (var secondPaint = new SKPaint())
            {
                secondPaint.IsAntialias = true;
                secondPaint.Style = SKPaintStyle.Fill;
                secondPaint.Color = CreateColor(255, 255, 255, (byte)(_opacity * 255));
                Canvas.DrawCircle(X, Y, _size / 3, secondPaint);
            }
        }

        public void Accelerate(float factor)
        {
            TargetVelocity = factor * _velocityY;
        }

        public void SetAcceleration(float factor)
        {
            TargetVelocity = factor * _baseVelocity;
        }

        public void Slide(float factor)
        {
           
            _velocityY *= factor;
        }
    }
}
