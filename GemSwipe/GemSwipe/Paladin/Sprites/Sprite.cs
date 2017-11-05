using System;
using GemSwipe.Data;
using GemSwipe.Paladin.Core;
using SkiaSharp;

namespace GemSwipe.Paladin.Sprites
{
    public class Sprite : SkiaView
    {
        public SpriteModel _spriteModel;
        public Sprite( string name, float x, float y, float width, float height) : base( x, y, height, width)
        {
            var sheetPath = "Resources/Graphics/"+name+".png";
            var  bitmap = ResourceLoader.LoadBitmapAsync(sheetPath).Result;
            var info = bitmap?.Info ?? SKImageInfo.Empty;
            if (bitmap == null || info.Width == 0 || info.Height == 0)
            {
                throw new ArgumentException($"Unable to load sprite sheet bitmap '{sheetPath}'.");
            }

            _spriteModel = new SpriteModel(bitmap, "toto", new SKSize(width, height));
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.Color = CreateColor(255, 255, 255);
                paint.IsAntialias = false;
                _spriteModel.Draw(Canvas, X, Y, Width, Height, paint: paint);
            }
        }
    }
}
