using System;
using System.Threading.Tasks;
using GemSwipe.Data.PlayerData;
using GemSwipe.Game.Effects;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Sprites;
using GemSwipe.Paladin.UIElements.Buttons;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Map
{
    public class LevelButton : SimpleButton, IButton
    {
        public int LevelId { get; set; }
        public LevelProgressStatus ProgressStatus { get; set; }
        public bool IsFinal { get; set; }
        private Sprite _buttonSprite;

        public LevelButton(float x, float y, float size, int levelId, LevelProgressStatus progressStatus, bool isFinal = false) : base (x, y, 0, size)
        {
            IsFinal = isFinal;
            LevelId = levelId;
            ProgressStatus = progressStatus;

            NormalColor = new SKColor(14, 0, 163);
            DownColor = new SKColor(79, 0, 163);
            ActivatedColor = new SKColor(184, 117, 255);

            _buttonSprite =  new Sprite(SpriteConst.LevelBase, 0, 0, size, size, new SKPaint { Color = new SKColor(255, 255, 255) });
            AddChild(_buttonSprite);
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

        public Task LevelCleared()
        {
            ProgressStatus = LevelProgressStatus.Completed;
            return Task.Delay(500);
        }

        public Task LevelUnlocked()
        {
            ProgressStatus = LevelProgressStatus.InProgress;
            var effect = new GemPopEffect(X, Y, Height, Width);
            AddChild(effect);
            return effect.Start();
        }

        protected override void Draw()
        {
            if(ProgressStatus ==LevelProgressStatus.Completed)
            {
                _opacity = 1;
            }
            else if (ProgressStatus == LevelProgressStatus.Locked)
            {
                _opacity = 0.5f;
            }
            else if (ProgressStatus == LevelProgressStatus.InProgress)
            {
                // Special Effect TODO
            }
            //Color = new SKColor(R, G, B);

            if (IsFinal )
            {
                var outerPaint = new SKPaint
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    Color = CreateColor(255, 255, 255),
                    StrokeWidth = Height / 200f,
                };
                 
                 Canvas.DrawCircle(X, Y, Height/2*1.35f, outerPaint);
            }

            _hitbox = SKRect.Create(X - Height, Y - Height, Height * 2, Height * 2);
        }
    }
}
