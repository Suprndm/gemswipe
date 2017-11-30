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
using GemSwipe.Game.Models;
using GemSwipe.Paladin.UIElements.Buttons;

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

            LevelDataRepository _levelDataRepository = new LevelDataRepository();
            LevelData levelData = _levelDataRepository.Get(6);
            var boardMarginTop = Height * 0.2f;
            Board _board = new Board(new BoardSetup(levelData), 0, 0 + boardMarginTop, Width, Height);
            AddChild(_board);

            TextButton textButton = new TextButton(width / 2, height-height/20, height / 30, "swipe");
            DeclareTappable(textButton);
            AddChild(textButton);
            textButton.Activated += () => _board.Swipe(Direction.Right);
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
