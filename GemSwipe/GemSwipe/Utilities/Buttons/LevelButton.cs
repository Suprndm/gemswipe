using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GemSwipe.Data.PlayerData;
using GemSwipe.Game.Effects.BackgroundEffects;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Utilities.Buttons
{
    public class LevelButton : SimpleButton, IButton
    {
        public int Level { get; set; }
        public LevelProgressStatus ProgressStatus { get; set; }

        public LevelButton(float x, float y, float height, int level, LevelProgressStatus progressStatus) : base (x, y, 0, height)
        {
            
            Level = level;
            ProgressStatus = progressStatus;

            NormalColor = new SKColor(14, 0, 163);
            DownColor = new SKColor(79, 0, 163);
            ActivatedColor = new SKColor(184, 117, 255);
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

            Color = new SKColor(R, G, B);
            var outerPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = CreateColor(255, 255, 255)
            };

            Canvas.DrawCircle(X, Y, Height*1.2f, outerPaint);

            SKColor ProgressColor = CreateColor(R, G, B);
            switch (ProgressStatus)
            {
                    case LevelProgressStatus.InProgress:
                        ProgressColor = Color;
                        break;
                    case LevelProgressStatus.Completed:
                        ProgressColor = CreateColor(R, G, 0);
                        break;
                    case LevelProgressStatus.Locked:
                        ProgressColor = CreateColor(R, R, R);
                        break;
            }

            var innerPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = ProgressColor
                //Color = CreateColor(Color)
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
                paint.Color = CreateColor(new SKColor(255,255,255));

                var textLenght = paint.MeasureText(Level.ToString());

                Width = textLenght;

                _hitbox = SKRect.Create(X - Height*2, Y - Height*2 , Height*4, Height*4);
                Canvas.DrawText(Level.ToString(), X - textLenght / 2, Y + Height / 4, paint);
            }
        }
    }
}
