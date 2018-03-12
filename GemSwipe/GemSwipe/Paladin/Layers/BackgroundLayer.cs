using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Navigation;

namespace GemSwipe.Paladin.Layers
{

    public class BackgroundLayer : SkiaView
    {
        private Background _background;

        public BackgroundLayer()
        {
            _background = new Background();
            AddChild(_background);

            Navigator.WorldChanged += Navigator_WorldChanged;
        }

        private void Navigator_WorldChanged(int worldId)
        {
            _background.OnWorldChanged(worldId);
        }

        protected override void Draw()
        {
        }
    }
}
