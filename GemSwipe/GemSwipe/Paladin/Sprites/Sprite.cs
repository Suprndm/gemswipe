using System;
using GemSwipe.Data;
using GemSwipe.Paladin.Core;
using GemSwipe.Services;
using SkiaSharp;

namespace GemSwipe.Paladin.Sprites
{
    public class Sprite : SkiaView
    {
        private SpriteModel _spriteModel;
        private SKPaint _paint;

        public Sprite(string spriteName, float x, float y, float width, float height, SKPaint paint = null) : base(x, y, height, width)
        {
            if (paint == null)
            {
                _paint = new SKPaint();
                _paint.Color = new SKColor(255, 255, 255);
                _paint.IsAntialias = true;
            }
            else
            {
                _paint = paint;
            }

            var spriteData = SpriteLoader.Instance.GetData(spriteName);
            _spriteModel = new SpriteModel(spriteData);
        }

        public void UpdateSprite(string spriteName)
        {
            var spriteData = SpriteLoader.Instance.GetData(spriteName);
            _spriteModel = new SpriteModel(spriteData);
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.Color = CreateColor(_paint.Color.Red, _paint.Color.Green, _paint.Color.Blue, _paint.Color.Alpha);
                paint.IsAntialias = _paint.IsAntialias;
                paint.BlendMode = _paint.BlendMode;

                _spriteModel.Draw(Canvas, X, Y, Width, Height, paint: paint);
            }
        }
    }
}
