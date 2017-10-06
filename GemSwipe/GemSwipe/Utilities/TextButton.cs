using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Utilities
{

    public class TextButton : SimpleButton
    {
        public string Text { get; set; }
        public float Size { get; set; }

        public TextButton(SKCanvas canvas, float x, float y, string text, float size, SKColor color) : base(canvas, x, y, size, size, color)
        {
            Text = text;
            Size = size;
            DeclareTappable(this);
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

                Width = textLenght;

                _hitbox = SKRect.Create(X - textLenght / 2, Y - Size / 3, Width, Size);
                Canvas.DrawText(Text, X - textLenght / 2, Y + Size / 2, paint);

            }
        }
    }
}
