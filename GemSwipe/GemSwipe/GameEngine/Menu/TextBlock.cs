using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Menu
{
    public class TextBlock:SkiaView
    {

        public string Text { get; set; }
        public float Size { get; set; }
        public SKColor Color { get; set; }


        public TextBlock(SKCanvas canvas, float x, float y, string text, float size, SKColor color) : base(canvas, x, y, size, size)
        {
            Text = text;
            Size = size;
            Color = color;
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.Yellow;
                paint.Typeface = SKTypeface.FromFamilyName(
                    "Arial",
                    SKFontStyleWeight.Bold,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Italic);

                paint.TextSize = Size;
                paint.IsAntialias = true;
                paint.Color = Color;

                var textLenght = paint.MeasureText(Text);

                Canvas.DrawText(Text, X - textLenght / 2, Y + Size / 2, paint);
            }
        }
    }
}
