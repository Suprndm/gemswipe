using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class OrbitingStarParticle : SkiaView
    {
        private float _radius { get; set; }
        private float _size { get; set; }
        private float _shift { get; set; }
        private float _speed { get; set; }
        private SKColor _color { get; set; }
        private float _centerX { get; set; }
        private float _centerY { get; set; }
        private float _phaseSpeedX { get; set; }
        private float _phaseSpeedY { get; set; }
        private float _velocityX { get; set; }
        private float _velocityY { get; set; }
        private float _targetX { get; set; }
        private float _targetY { get; set; }

        public OrbitingStarParticle(float x, float y, float orbit, Random randomizer, SKColor color) : base(x, y, 10, 10)
        {
            _centerX = x;
            _centerY = y;
            _radius = randomizer.Next((int)(orbit), (int)(4 * orbit / 2));
            float depthZ = randomizer.Next(1, 7);
            _size = (7 - depthZ);
            _shift = (float)Math.PI * randomizer.Next(360) / 360;
            _speed = (float)Math.PI * randomizer.Next(120, 360) / (360 * 50);
            _color = color;
            _phaseSpeedX = 1;
            _phaseSpeedY = randomizer.Next(1, 3);
        }

        public async Task SteerToTarget()
        {
            this.Animate("moveX", p => _centerX = (float)p, _centerX, _targetX, 8, (uint)1000, Easing.SinInOut);
            this.Animate("moveY", p => _centerY = (float)p, _centerY, _targetY, 8, (uint)1000, Easing.SinInOut);
            await Task.Delay(0);

        }

        public async Task SlowPulse()
        {

            //_color.ToHsl();
            //SKColor.FromHsl()
            //    hsla(29, 93 %, 45 %, 1)
            //rgba(244, 160, 16, 1)

            float baseSize = _size;
            SKColor baseColor = _color;
            float hHsl;
            float sHsl;
            float lHslBase;
            float lHslCurrent;
            float targetLuminosity = 40;

            baseColor.ToHsl(out hHsl, out sHsl, out lHslCurrent);
            lHslBase = lHslCurrent;
            this.Animate("LightColor", p => _color = SKColor.FromHsl(hHsl, sHsl, (float)p), lHslCurrent, targetLuminosity, 8, (uint)1000, Easing.SinInOut);
            this.Animate("Grow", p => _size = (float)p, _size, 1.4f * _size, 8, (uint)1000, Easing.SinInOut);

            await Task.Delay(1000);

            _color.ToHsl(out hHsl, out sHsl, out lHslCurrent);
            this.Animate("FadeColor", p => _color = SKColor.FromHsl(hHsl, sHsl, (float)p), lHslCurrent, lHslBase, 8, (uint)1000, Easing.SinInOut);
            this.Animate("Recede", p => _size = (float)p, _size, baseSize, 8, (uint)1000, Easing.SinInOut);

            await Task.Delay(2000);
        }

        public void SetTarget(float x, float y)
        {
            _targetX = x;
            _targetY = y;
        }
        public void ApplySteerArrive(float targetX, float targetY)
        {
            float desiredX = targetX - _centerX;
            float desiredY = targetY - _centerY;
            float d = (float)Math.Sqrt(desiredX * desiredX + desiredY * desiredY);

            float arriveRadius = 100;
            float maxSpeed = 25;
            float m = 0;

            if (d < arriveRadius)
            {
                m = d * maxSpeed / arriveRadius;
            }
            else
            {
                m = maxSpeed;
            }

            desiredX = desiredX * m / d;
            desiredY = desiredY * m / d;
            float steerX = desiredX - _velocityX;
            float steerY = desiredY - _velocityY;

            float mag = (float)Math.Sqrt(steerX * steerX + steerY * steerY);

            float maxSteer = 1;


            if (mag > maxSteer)
            {
                steerX = maxSteer * steerX / mag;
                steerY = maxSteer * steerY / mag;
            }
            _velocityX += steerX;
            _velocityY += steerY;
        }

        public void Update()
        {
            _shift += _speed;
            _x = _centerX + _radius * (float)Math.Cos(_phaseSpeedX * _shift);
            _y = _centerY + _radius * (float)Math.Sin(_phaseSpeedY * _shift);
            _opacity = (float)(Math.Cos(_phaseSpeedY * _shift) + 1) / 2;
        }

        protected override void Draw()
        {
            Update();

            using (var secondPaint = new SKPaint())
            {
                secondPaint.IsAntialias = true;
                secondPaint.Style = SKPaintStyle.Fill;
                //secondPaint.Color = CreateColor(255, 255, 255, (byte)(_opacity * 255));
                secondPaint.Color = _color;
                Canvas.DrawCircle(X, Y, _size, secondPaint);
            }
        }
    }
}
