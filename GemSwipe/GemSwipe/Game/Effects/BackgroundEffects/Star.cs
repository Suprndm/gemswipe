using System;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Sprites;
using GemSwipe.Services;
using SkiaSharp;
using Xamarin.Forms;

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

        private readonly Sprite _spriteDot;
        private readonly Sprite _spriteHalo;

        public Star(Random randomizer, float height, float width) : base(0, 0, height, width)
        {
            _direction = 1;
            _randomizer = randomizer;

            _x = _randomizer.Next((int)Width);

            _y = _randomizer.Next((int)Height);
            _velocityY = _direction * _randomizer.Next((int)(5 * Height / 2000), (int)(8 * Height / 2000));
            _baseVelocity = _velocityY;
            TargetVelocity = _velocityY;

            Opacity = Opacity *(_randomizer.Next(100) / 100f);

            _spriteDot = new Sprite(SpriteConst.SmallWhiteDot, Width / 2, Height / 2, _size, _size, new SKPaint { Color = CreateColor(255, 255, 255) });
            _spriteHalo = new Sprite(SpriteConst.SmallWhiteHalo, Width / 2, Height / 2, _size * _size, _size * _size, new SKPaint { Color = CreateColor(255, 255, 255), BlendMode = SKBlendMode.Plus});
            AddChild(_spriteDot);
            AddChild(_spriteHalo);

            ResetRandomCinematicProperties();
        }

        public void ResetRandomCinematicProperties()
        {
            _depthZ = _randomizer.Next(1, 10);
            _size = (14 - _depthZ);

            _phase = _randomizer.Next(400) / 100;
            _speed = _randomizer.Next(10) / 100f;

            _spriteHalo.Width = _size * _size*4;
            _spriteHalo.Height = _size * _size*4;

            _spriteDot.Width = _size/2 + 3;
            _spriteDot.Height = _size / 2 + 3;

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

            _phase += _speed * 0.2f;
            _opacity = (float)(Math.Cos(_phase) + 1) / 2;
            _accelerationY = 0;
        }

        protected override void Draw()
        {
            Update();

            _spriteDot.X = X;
            _spriteDot.Y = Y;


            _spriteHalo.X = X;
            _spriteHalo.Y = Y;

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
