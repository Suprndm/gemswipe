using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Effects
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
