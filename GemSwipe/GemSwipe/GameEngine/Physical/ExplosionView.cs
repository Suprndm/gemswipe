using System;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Physical
{
    public class ExplosionView : SkiaView
    {
        private const int ParticuleNumbers = 100;
        private readonly float G;
        private readonly float F;
        private readonly float K;
        public ExplosionView(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            G = 1f;
            F = 2f;
            K = 0.95f;
            var randomizer = new Random();
            for (int i = 0; i < ParticuleNumbers; i++)
            {
                var randomValue = randomizer.Next(1, 10) / 10f;
                AddChild(new PhysicalParticule(G, K, randomizer.Next(360), F * randomValue, canvas, 100, 0, height / 100 * randomValue, height / 100 * randomValue));
            }
        }

        protected override void Draw()
        {
        }
    }
}
