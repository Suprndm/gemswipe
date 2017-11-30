using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Shards;
using GemSwipe.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GemSwipe.Game.Models.BoardModel.Gems
{
    public class BlackholeGem : Gem
    {
        private int _animationMs = 600;
        private IList<OrbitingStarParticle> _listOfStarParticles;

        public BlackholeGem(int boardX, int boardY, int size, Board board) : base(boardX, boardY, size,board)
        {
            Type = GemType.Blackhole;
            _listOfStarParticles = new List<OrbitingStarParticle>();
            ActivateOrbitingStars(SkiaRoot.ScreenWidth, SkiaRoot.ScreenHeight);
        }
        public BlackholeGem(int boardX, int boardY, int size, float x, float y, float radius, Random randomizer,Board board) : base(boardX, boardY, size, x, y, radius, randomizer,board)
        {
            Type = GemType.Blackhole;
            _listOfStarParticles = new List<OrbitingStarParticle>();
            ActivateOrbitingStars(SkiaRoot.ScreenWidth, SkiaRoot.ScreenHeight);
        }

        protected override async void Shine()
        {
            Shard shard = new Shard(Radius, Radius, 2 * Radius, 2 * Radius, CreateColor(0, 0, 0));
            AddChild(shard);
            await Task.Delay(_animationMs / 2);
            shard.Die();
        }

        public async Task Swallow()
        {
            foreach (OrbitingStarParticle star in _listOfStarParticles)
            {
                star.SlowPulse();
            }
            Shard shard = new Shard(Radius, Radius, Radius, Radius);
            AddChild(shard);
            await Task.Delay(_animationMs / 2);
            shard.Die();
        }

        public void ActivateOrbitingStars(float screenWidth, float screenHeight)
        {

            Random randomizer = new Random();
            for (int i = 0; i < 30; i++)
            {
                OrbitingStarParticle star = new OrbitingStarParticle(randomizer.Next((int)screenWidth) + screenWidth / 2,
                    randomizer.Next((int)screenHeight) + screenHeight / 2,
                    Radius / 4,
                    randomizer,
                    SKColor.FromHsl(21, 78, randomizer.Next(0, 6))
                    );
                //OrbitingStarParticle star = new OrbitingStarParticle(randomizer.Next((int)screenWidth) - screenWidth / 2, randomizer.Next((int)screenHeight) - screenHeight / 2, Radius / 4, randomizer, CreateColor(0, 0, 0));
                star.SetTarget(Radius, Radius);
                AddChild(star);
                _listOfStarParticles.Add(star);
                star.SteerToTarget();

            }
        }

        protected override void Draw()
        {
            FloatingParticule.Update();
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.StrokeWidth = 2;
                paint.Color = CreateColor(255, 200, 36);
                //Canvas.DrawCircle(X + Radius + FloatingParticule.X, Y + Radius + FloatingParticule.Y, Radius, paint);
            }
        }
    }
}