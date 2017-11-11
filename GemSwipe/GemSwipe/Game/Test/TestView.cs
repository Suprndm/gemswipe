using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Events;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Settings;
using GemSwipe.Paladin.UIElements;
using Newtonsoft.Json;
using GemSwipe.Game.Models.BoardModel.Gems;

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

            AddChild(new BlackholeGem(1, 1, 1, Width / 2-100/2, Height / 2-100/2, 100, new System.Random()));
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
