using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Effects.BackgroundEffects;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Utilities.Buttons
{
    public class LevelButton : SimpleButton, IButton
    {
        public string Text { get; set; }
        public object Info { get; set; }



        public LevelButton(SKCanvas canvas, float x, float y, float height, string text, object info, SKColor color) : base(canvas, x, y, 0, height, color)
        {
            Text = text;
            Info = info;
            //Tapped+=ActivateOrbitingStars;

        }

        public void ActivateOrbitingStars(float screenWidth, float screenHeight)
        {
            Random randomizer = new Random();
            for (int i = 0; i < 20; i++)
            {
                OrbitingStarParticle star = new OrbitingStarParticle(Canvas, randomizer.Next((int)screenWidth) - screenWidth / 2, randomizer.Next((int)screenHeight) - screenHeight / 2, Height, randomizer, CreateColor(255, 255, 255));
                star.SetTarget(0, 0);
                AddChild(star);
                star.SteerToTarget();

            }
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                //paint.Color = SKColors.Yellow;
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
