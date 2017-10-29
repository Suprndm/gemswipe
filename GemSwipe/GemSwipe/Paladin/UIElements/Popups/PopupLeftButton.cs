using GemSwipe.Paladin.UIElements.Buttons;
using SkiaSharp;

namespace GemSwipe.Paladin.UIElements.Popups
{
    public class PopupLeftButton : SimpleButton
    {
        public float PopupHeight { get; set; }
        public float PopupWidth { get; set; }
        public float Radius { get; set; }

        public PopupLeftButton(float x, float y, float width, float height, float popupWith, float popupHeight, float radius) : base(x, y, width, height)
        {
            PopupWidth = popupWith;
            PopupHeight = popupHeight;
            Radius = radius;
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(R, G, B);
                using (new SKAutoCanvasRestore(Canvas))
                {
                    Canvas.ClipRect(SKRect.Create(X, Y, Width, Height), antialias: true);
                    Canvas.DrawRoundRect(SKRect.Create(X, Y - PopupHeight + Height, PopupWidth, PopupHeight), Radius,
                        Radius, paint);
                }
            }


            using (var textPaint = new SKPaint())
            {
                textPaint.Color = SKColors.Yellow;
                textPaint.Typeface = SKTypeface.FromFamilyName(
                    "Arial",
                    SKFontStyleWeight.Light,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Upright);

                textPaint.TextSize = Height / 3;
                textPaint.IsAntialias = true;
                textPaint.Color = CreateColor(0, 0, 0);

                var textLenght = textPaint.MeasureText("X");

                Canvas.DrawText("X", X - textLenght / 2 + Width / 2, Y + Height / 2 + textPaint.TextSize/3, textPaint);
            }


            _hitbox = SKRect.Create(X, Y, Width, Height);
        }


    }
}
