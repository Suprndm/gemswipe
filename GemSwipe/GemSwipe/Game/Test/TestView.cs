using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Entities;
using GemSwipe.Game.Pages.Game;
using Newtonsoft.Json;

namespace GemSwipe.Game.Test
{
    public class TestView : SkiaRoot
    {
        private float _angle = 0;
        private TextBlock _fpsText;
        public TestView(float x, float y, float height, float width) : base(x, y, height, width)
        {
            var skiaRoot = new SkiaRoot(0, 0, height, width);

            _fpsText = new TextBlock(width / 2, width / 40, "0", width / 40, CreateColor(255, 255, 255));
            AddChild(_fpsText);

          //  AddChild(new PopupTester(Height, Width));

            var objectives = new Dictionary<int, int>();
            objectives.Add(2,8);
            objectives.Add(4,5);
            objectives.Add(6,1);
            objectives.Add(8,3);

            var levelData = new LevelData()
            {
                BoardSetupString = "",
                Columns = 4,
                Rows = 4,
                Id = 1,
                Moves = 17,
                Objectives = objectives
            };

            var json = JsonConvert.SerializeObject(levelData);
            var objectivesView = new ObjectivesView(objectives, true, width / 2, 0.1f * height, 0.1f * height);

            AddChild(objectivesView);

        }


        public override void SetupLayers()
        {
            // do nothing
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
