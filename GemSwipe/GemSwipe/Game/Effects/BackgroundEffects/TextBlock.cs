using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class TextBlock:SkiaView
    {
        public string Text { get; set; }
        public float Size { get; set; }
        public SKColor Color { get; set; }
        private SKRect _hitbox;
        public TextBlock( float x, float y, string text, float size, SKColor color) : base( x, y, size, size)
        {
            Text = text;
            Size = size;
            Color = color;
        }

        protected override void Draw()
        {

            if(string.IsNullOrEmpty(Text))
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

                _hitbox = SKRect.Create(X - textLenght / 2, Y- Size / 3, Width, Size);
                Canvas.DrawText(Text, X - textLenght / 2, Y + Size / 2, paint);

            }
        }

        public override SKRect GetHitbox()
        {
            return _hitbox;
        }
    }
}
