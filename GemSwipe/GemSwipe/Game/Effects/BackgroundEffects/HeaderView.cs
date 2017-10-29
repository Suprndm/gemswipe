using GemSwipe.Paladin.Core;
using SkiaSharp;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class HeaderView : SkiaView
    {
        public CountDownView CountDownView { get; set; }

        public HeaderView( float x, float y, float height, float width) : base( x, y, height,
            width)
        {
            var countDownWidth = width * 0.2f;
            var countDownHeight = height;

            CountDownView = new CountDownView( width / 2 - countDownWidth / 2, y, countDownHeight,
                countDownWidth);
            AddChild(CountDownView);
        }

        protected override void Draw()
        {
            //    var cellColor = new SKPaint
            //    {
            //        IsAntialias = true,
            //        Style = SKPaintStyle.Fill,
            //        Color = SKColor.FromHsl(233, 35, 23, 33)
            //    };

            //    Canvas.DrawRect(
            //        SKRect.Create(
            //            X,
            //            Y,
            //            Width,
            //            Height),
            //        cellColor);
            //}
        }

    }
}