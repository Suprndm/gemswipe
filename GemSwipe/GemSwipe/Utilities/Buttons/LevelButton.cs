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
        public int Level { get; set; }



        public LevelButton(float x, float y, float height, int level, SKColor color) : base (x, y, 0, height, color)
        {
            Level = level;
            //Tapped+=ActivateOrbitingStars;

        }

        public void ActivateOrbitingStars(float screenWidth, float screenHeight)
        {
            Random randomizer = new Random();
            for (int i = 0; i < 20; i++)
            {
                OrbitingStarParticle star = new OrbitingStarParticle(randomizer.Next((int)screenWidth) - screenWidth / 2, randomizer.Next((int)screenHeight) - screenHeight / 2, Height, randomizer, CreateColor(255, 255, 255));
                star.SetTarget(0, 0);
                AddChild(star);
                star.SteerToTarget();

            }
        }

        protected override void Draw()
        {

            var outerPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = CreateColor(255, 255, 255)
            };

            Canvas.DrawCircle(X, Y, Height*1.2f, outerPaint);

            var innerPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = CreateColor(14, 0, 163)
            };

            Canvas.DrawCircle(X, Y, Height, innerPaint);


            using (var paint = new SKPaint())
            {
                //paint.Color = SKColors.Yellow;
                paint.Typeface = SKTypeface.FromFamilyName(
                    "Courier New",
                    SKFontStyleWeight.Bold,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Upright);

                paint.TextSize = Height;
                paint.IsAntialias = true;
                paint.Color = CreateColor(Color);

                var textLenght = paint.MeasureText(Level.ToString());

                Width = textLenght;

                _hitbox = SKRect.Create(X - textLenght / 2, Y - Height / 3, Width, Height);
                Canvas.DrawText(Level.ToString(), X - textLenght / 2, Y + Height / 4, paint);

            }
        }
    }
}
