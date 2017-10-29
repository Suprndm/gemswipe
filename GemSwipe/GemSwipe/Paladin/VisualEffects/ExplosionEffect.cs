using System;
using GemSwipe.Paladin.Core;

namespace GemSwipe.Paladin.VisualEffects
{
    public class ExplosionEffect:SkiaView
    {
        private readonly float G;
        private readonly float F;
        private readonly float K;
        public ExplosionEffect( float x, float y, float height, float width, float nbParticules, float f, float k) : base( x, y, height, width)
        {
            G = 0;
            F = f;
            K = k;
            var randomizer = new Random();
            for (int i = 0; i < nbParticules; i++)
            {
                var randomValue = randomizer.Next(1, 100) / 100f;
                AddChild(new PhysicalParticule(G, K, randomizer.Next(360), F * randomValue, 0, 0, 1, 1));
            }
        }

      

        protected override void Draw()
        {
            
        }
    }
}
