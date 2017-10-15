using System;
using System.Diagnostics.Contracts;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Navigation;
using GemSwipe.Utilities;
using GemSwipe.Utilities.Buttons;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game.Floors
{
    public class EndFloor : Floor
    {
        private readonly int _levelId;

        private TextButton _nextLevelButton;
        private TextButton _backButton;
        public EndFloor( float x, float y, float height, float width, int levelId) : base( x, y, height, width)
        {
            _levelId = levelId;
            _nextLevelButton = new TextButton( Width / 2, Height * .6f, Width / 30, $"Go to next level");
            AddChild(_nextLevelButton);
            _nextLevelButton.Activated += NextLevelBlock_Tapped;

            _backButton = new TextButton( Width / 2, Height * .8f, Width / 30, $"Go back to the map");
            AddChild(_backButton);
            _backButton.Activated += BackBlock_Tapped;

            AddChild(new TextBlock( Width / 2, Height * .4f, $"Congratulation !", (int)Width / 10, CreateColor(255, 255, 255, 255)));
        }

        private void NextLevelBlock_Tapped()
        {
            Navigator.Instance.GoTo(PageType.Game, _levelId + 1);
        }

        private void BackBlock_Tapped()
        {
            Navigator.Instance.GoTo(PageType.Map);

        }

        public override void Dispose()
        {
            _nextLevelButton.Activated -= NextLevelBlock_Tapped;
            _backButton.Activated -= BackBlock_Tapped;
        }

        protected override void Draw()
        {
            base.Draw();
        }
    }
}
