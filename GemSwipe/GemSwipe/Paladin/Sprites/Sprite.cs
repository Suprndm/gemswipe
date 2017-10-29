using GemSwipe.Paladin.Core;
using SkiaSharp;

namespace GemSwipe.Paladin.Sprites
{
    public class Sprite : SkiaView
    {
        public SpriteModel spriteModel;
        public Sprite( string name, float x, float y, float width, float height) : base( x, y, height, width)
        {
            spriteModel = SpriteSheet.Instance.Sprites[name];
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.Color = CreateColor(255, 255, 255);
                paint.IsAntialias = true;
                spriteModel.Draw(Canvas, X, Y, Width, Height, paint: paint);
            }
        }
    }
}
