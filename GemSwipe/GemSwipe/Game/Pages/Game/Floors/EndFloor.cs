﻿using System.Diagnostics.Contracts;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Navigation;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game.Floors
{
    public class EndFloor : Floor
    {
        private readonly int _levelId;

        public EndFloor( float x, float y, float height, float width, int levelId) : base( x, y, height, width)
        {
            _levelId = levelId;
            var nextLevelBlock = new TextBlock( Width / 2, Height * .6f, $"Go to next level", (int)Width / 30,
                CreateColor(255, 255, 255, 255));
            AddChild(nextLevelBlock);
            nextLevelBlock.DeclareTappable(nextLevelBlock);
            nextLevelBlock.Tapped += NextLevelBlock_Tapped;

            var backBlock = new TextBlock( Width / 2, Height * .8f, $"Go back to the map", (int)Width / 30,
                CreateColor(255, 255, 255, 255));
            AddChild(backBlock);
            backBlock.DeclareTappable(backBlock);
            backBlock.Tapped += BackBlock_Tapped;

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
            base.Dispose();
        }

        protected override void Draw()
        {
            base.Draw();
        }
    }
}
