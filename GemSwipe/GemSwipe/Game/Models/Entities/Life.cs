using System;
using GemSwipe.Paladin.Core;
using SkiaSharp;

namespace GemSwipe.Game.Models.Entities
{
    public class Life : SkiaView
    {
        private bool _started;
        public event Action Zero;
        public int Level { get; private set; }

        private const float BaseExperience = 1000;
        private const float LevelUpExperianceRatio = 0.33f;
        private double _experience;

        public Life( float x, float y, float height, float width) : base( x, y, height, width)
        {
            Reset();
        }

        public void Reset()
        {
            Level = 0;
            _experience = 0;
            LevelUp();
        }

        public void Start()
        {
            _started = true;
        }

        private void LevelUp()
        {
            Level++;
            _experience = BaseExperience * LevelUpExperianceRatio;
        }

        private void Consume()
        {
            _experience += -BaseExperience * 0.00005 * Level;
            if (_experience <= 0)
            {
                Zero?.Invoke();
                _started = false;
            }
        }

        public void BoardFinished(bool perfectSolved)
        {
            if (perfectSolved)
                _experience += BaseExperience / 3;
            else
            {
                _experience += BaseExperience / 5;
            }

            if (_experience > BaseExperience) LevelUp();
        }

        protected override void Draw()
        {
            if (_started)
            {
                Consume();
            }
            using (var paint = new SKPaint())
            {
                var text = $"LevelData {Level}";
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

                Canvas.DrawText(text, X + Width / 2 + -test / 2, Y + Height / 2 + paint.TextSize / 2, paint);
            }

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = CreateColor(255, 255, 255);
                Canvas.DrawRect(
                    SKRect.Create(
                      X, Y, Width * (float)_experience / BaseExperience, Height * 0.1f),
                    paint);
            }
        }
    }
}
