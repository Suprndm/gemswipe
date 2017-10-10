using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class LinearGradientBackground : SkiaView
    {
        private SKColor _color1;
        private SKColor _color2;
        public LinearGradientBackground( float x, float y, float height, float width) : base( x, y, height, width)
        {
        }

        protected override void Draw()
        {
            var colors = new SKColor[]
            {
                _color1,
                _color2
            };

            var shader = SKShader.CreateLinearGradient(new SKPoint(X, Y), new SKPoint(X, Y + Height), colors, null,
                SKShaderTileMode.Clamp);
            var paint = new SKPaint() { Shader = shader };
            paint.IsAntialias = true;
            Canvas.DrawRect(SKRect.Create(X, Y, Width, Height), paint);
        }

        public void Reset(SKColor color1, SKColor color2, float y)
        {
            _color1 = color1;
            _color2 = color2;
            _y = y;
        }
    }
}
