using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Layers;
using GemSwipe.Paladin.Navigation;

namespace GemSwipe.Game.Test
{
    class BackgroundTester : SkiaView
    {
        BackgroundLayer _background;
        int count;
        public BackgroundTester()
        {
            _background = new BackgroundLayer();
            AddChild(_background);

            DeclareTappable(this);
            Up += BackgroundTester_Up;
        }

        private void BackgroundTester_Up()
        {
            count++;
            if (count > 3) count = 1;

            Navigator.Instance.ChangeWorld(count);
        }

        protected override void Draw()
        {
        }
    }
}
