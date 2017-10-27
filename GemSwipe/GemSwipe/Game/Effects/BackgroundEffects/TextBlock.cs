using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class TextBlock : SkiaView
    {
        public string Text { get; set; }
        public float Size { get; set; }
        public SKColor Color { get; set; }
        private SKRect _hitbox;
        private HorizontalAlignment _horizontalAlignment;

        public TextBlock(float x, float y, string text, float size, SKColor color, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center) : base(x, y, size, size)
        {
            _horizontalAlignment = horizontalAlignment;
            Text = text;
            Size = size;
            Color = color;
        }

        protected override void Draw()
        {

            if (string.IsNullOrEmpty(Text))
                return;

            using (var paint = new SKPaint())
            {
                paint.Typeface = SKTypeface.FromFamilyName(
                    "Arial",
                    SKFontStyleWeight.Light,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Upright);

                paint.TextSize = Size;
                paint.IsAntialias = true;
                paint.Color = CreateColor(Color);

                var textLenght = paint.MeasureText(Text);

                Width = textLenght;

                float xModifier = 0;

                if (_horizontalAlignment == HorizontalAlignment.Center)
                    xModifier = -textLenght / 2;
                else if (_horizontalAlignment == HorizontalAlignment.Left)
                    xModifier = -textLenght;

                _hitbox = SKRect.Create(X + xModifier, Y - Size / 3, Width, Size);
                Canvas.DrawText(Text, X + xModifier, Y + Size / 2, paint);

            }
        }

        public override SKRect GetHitbox()
        {
            return _hitbox;
        }
    }
}
