using GemSwipe.Game.Pages.Home;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Utilities.Buttons
{
    public class TextButton : SimpleButton
    {
        public string Text { get; set; }
        public object Info { get; set; }
        public SKColor TextColor { get; set; }
        protected float Padding { get; set; }

        public TextButton(float x, float y, float height, string text) : base(x, y, 0, height)
        {
             TextColor = new SKColor(255, 255, 255);
            Text = text;
            Padding = height /2;
        }

        protected override void Draw()
        {
            Color = CreateColor(R, G, B);

            using (var textPaint = new SKPaint())
            {
                textPaint.Color = SKColors.Yellow;
                textPaint.Typeface = SKTypeface.FromFamilyName(
                    "Arial",
                    SKFontStyleWeight.Light,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Upright);

                textPaint.TextSize = Height;
                textPaint.IsAntialias = true;
                textPaint.Color = TextColor;

                var textLenght = textPaint.MeasureText(Text);

                Width = textLenght;


                var blockWidth = Width + 2 * Padding;
                var blockHeight = Height + 2 * Padding;

                _hitbox = SKRect.Create(X - blockWidth/2, Y - Height / 3 - Padding, blockWidth, blockHeight);

                using (var blockPaint = new SKPaint())
                {
                    blockPaint.Color = Color;
                    blockPaint.IsAntialias = true;
                    blockPaint.Style = SKPaintStyle.Fill;

                    Canvas.DrawRect(GetHitbox(), blockPaint);

                    Canvas.DrawCircle(X - blockWidth / 2, Y - Height / 3 - Padding + blockHeight / 2, blockHeight / 2, blockPaint);
                    Canvas.DrawCircle(X + blockWidth / 2, Y - Height / 3 - Padding + blockHeight / 2, blockHeight / 2, blockPaint);
                }

                Canvas.DrawText(Text, X - textLenght / 2, Y + Height / 2, textPaint);
            }
        }
    }
}

