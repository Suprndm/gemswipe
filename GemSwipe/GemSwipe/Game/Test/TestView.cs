using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.UIElements;

namespace GemSwipe.Game.Test
{
    public class TestView : SkiaRoot
    {
        private float _angle = 0;
        private TextBlock _fpsText;

        public TestView(float x, float y, float height, float width) : base(x, y, height, width)
        {
            _fpsText = new TextBlock(Width / 2, Width / 40, "0", Width / 40, CreateColor(255, 255, 255));
            AddChild(_fpsText);

            AddChild(new SpriteTester());
        }

        public override void SetupLayers()
        {

        }

        public override void UpdateFps(long fps)
        {
            _fpsText.Text = fps.ToString();
        }

        protected override void Draw()
        {

        }


    }
}
