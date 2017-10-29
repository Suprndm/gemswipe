using GemSwipe.Paladin.Core;
using SkiaSharp;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class CountDownView : SkiaView
    {
        public double RemainingSeconds { get; set; }

        public CountDownView( float x, float y, float height, float width) : base( x, y, height, width)
        {
        }


        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                var text=  RemainingSeconds.ToString("###");
                paint.TextSize = 24;
                paint.Color = SKColors.Yellow;
                paint.Typeface = SKTypeface.FromFamilyName(
                    "Arial",
                    SKFontStyleWeight.Bold,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Italic);

                paint.TextSize = Height / 2f;
                paint.IsAntialias = true;
                paint.Color = CreateColor(255, 255, 255, 255);

                var test = paint.MeasureText(text);

                Canvas.DrawText(RemainingSeconds.ToString("###"), X + Width / 2 + -test/2, Y + Height / 2 + paint.TextSize/2, paint);
            }
        }
    }
}
