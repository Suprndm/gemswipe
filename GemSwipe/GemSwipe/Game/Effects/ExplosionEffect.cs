using System;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Effects
{
    public class ExplosionEffect:SkiaView
    {
        private const int ParticuleNumbers = 500;
        private readonly float G;
        private readonly float F;
        private readonly float K;
        public ExplosionEffect(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            G = 0;
            F = 50f;
            K = 0.98f;
            var randomizer = new Random();
            for (int i = 0; i < ParticuleNumbers; i++)
            {
                var randomValue = randomizer.Next(1, 100) / 100f;
                AddChild(new PhysicalParticule(G, K, randomizer.Next(360), F * randomValue, canvas, 0, 0, 1, 1));
            }
        }

      

        protected override void Draw()
        {
            
        }
    }
}
