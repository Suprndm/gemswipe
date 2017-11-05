using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Events;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Paladin.UIElements;
using Newtonsoft.Json;

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

            AddChild(new ShardTester());
        }

        public override void SetupLayers()
        {

        }

        public override void UpdateFps(long fps)
        {
            _fpsText.Text = fps.ToString();
        }

        public override Task LoadResources()
        {
            // do nothing
            return Task.Delay(0);
        }


        protected override void Draw()
        {

        }


    }
}
