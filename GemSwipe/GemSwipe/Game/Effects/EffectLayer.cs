using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Effects
{
    public  class EffectLayer:SkiaView
    {
        public EffectLayer(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
        }

        private void EffectLayer_Tapped()
        {
            
        }

        public void Explode()
        {
            AddChild(new ExplosionEffect(Canvas, Width / 2, Height / 2, 1, 1));
        }

        protected override void Draw()
        {
            
        }
    }
}
