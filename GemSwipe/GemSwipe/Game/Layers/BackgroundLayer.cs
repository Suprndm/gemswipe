using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.SkiaEngine;

namespace GemSwipe.Game.Layers
{

    public class BackgroundLayer : SkiaView
    {
        private Background _background;

        public BackgroundLayer()
        {
            _background = new Background();
            AddChild(_background);
        }

        protected override void Draw()
        {
        }
    }
}
