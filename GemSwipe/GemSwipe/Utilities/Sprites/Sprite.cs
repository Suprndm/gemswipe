using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Utilities.Sprites
{
    public class Sprite : SkiaView
    {
        public SpriteModel spriteModel;
        public Sprite(SKCanvas canvas, string name, float x, float y, float width, float height) : base(canvas, x, y, height, width)
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
