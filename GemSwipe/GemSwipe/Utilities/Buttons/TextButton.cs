using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Utilities;
using SkiaSharp;

namespace GemSwipe.Utilities
{
    public class TextButton : SimpleButton
    {
        public string Text { get; set; }

        public TextButton(SKCanvas canvas, float x, float y, float width, float height, string text, SKColor color) : base(canvas, x, y, width, height, color)
        {
            Text = text;
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

                paint.TextSize = Height;
                paint.IsAntialias = true;
                paint.Color = Color;

                var textLenght = paint.MeasureText(Text);

                Width = textLenght;

                _hitbox = SKRect.Create(X - textLenght / 2, Y - Height / 3, Width, Height);
                Canvas.DrawText(Text, X - textLenght / 2, Y + Height / 2, paint);

            }
        }
    }
}

