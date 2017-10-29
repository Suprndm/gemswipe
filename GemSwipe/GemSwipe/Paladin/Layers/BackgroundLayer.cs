using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Paladin.Core;

namespace GemSwipe.Paladin.Layers
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
