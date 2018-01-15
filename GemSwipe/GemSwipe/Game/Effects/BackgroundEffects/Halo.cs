using System;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Sprites;
using SkiaSharp;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class Halo : SkiaView
    {
        private const double Speed = 0.005;
        public double Angle { get; private set; }
        public SKColor Color { get; }

        private readonly Sprite _sprite;

        public Halo(string spriteName, float x, float y, float height, float width, SKColor color, double angle) : base( x, y, height, width)
        {
            Color = color;
            Angle = angle;


            _sprite = new Sprite(spriteName, Width / 2, Height / 2, width*4, width*4, new SKPaint { Color = CreateColor(Color.Red, Color.Green, Color.Blue, 150), BlendMode = SKBlendMode.Plus });
            AddChild(_sprite);
        }

        protected override void Draw()
        {
            Angle += Speed;

            _y = (float)Math.Sin(Angle) * Height*0.8f + Height / 3;
            _x = (float) Math.Cos(Angle) * Width / 3 + Width/8;

            var colors = new SKColor[] {
                CreateColor(Color.Red, Color.Green, Color.Blue, 255),
                CreateColor(Color.Red, Color.Green, Color.Blue, 0),
            };

            var shader = SKShader.CreateRadialGradient(new SKPoint(X, Y), Width / 2, colors, new[] { 0.0f, 1f }, SKShaderTileMode.Clamp);

            var paint = new SKPaint()
            {
                Shader = shader,
                BlendMode =  SKBlendMode.Luminosity,
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                DeviceKerningEnabled = true,
                
            };

            _sprite.X = X;
            _sprite.Y = Y;
        }
    }
}

