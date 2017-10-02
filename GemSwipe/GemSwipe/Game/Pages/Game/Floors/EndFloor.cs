using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Navigation;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game.Floors
{
    public class EndFloor : Floor
    {
        private readonly int _levelId;

        public EndFloor(SKCanvas canvas, float x, float y, float height, float width, int levelId) : base(canvas, x, y, height, width)
        {
            _levelId = levelId;
            var nextLevelBlock = new TextBlock(Canvas, Width / 2, Height * .6f, $"Go to next level", (int)Width / 30,
                new SKColor(255, 255, 255, 255));
            AddChild(nextLevelBlock);
            nextLevelBlock.DeclareTappable(nextLevelBlock);
            nextLevelBlock.Tapped += NextLevelBlock_Tapped;

            var backBlock = new TextBlock(Canvas, Width / 2, Height * .8f, $"Go back to the map", (int)Width / 30,
                new SKColor(255, 255, 255, 255));
            AddChild(backBlock);
            backBlock.DeclareTappable(backBlock);
            backBlock.Tapped += BackBlock_Tapped;

            AddChild(new TextBlock(Canvas, Width / 2, Height * .4f, $"Congratulation !", (int)Width / 10, new SKColor(255, 255, 255, 255)));
        }

        private void NextLevelBlock_Tapped()
        {
            Navigator.Instance.GoTo(PageType.Game, _levelId + 1);
        }

        private void BackBlock_Tapped()
        {
            Navigator.Instance.GoTo(PageType.Map);
        }

        protected override void Draw()
        {
            base.Draw();
        }
    }
}
