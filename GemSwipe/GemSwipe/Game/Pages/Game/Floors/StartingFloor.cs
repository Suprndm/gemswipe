using System.Threading.Tasks;
using GemSwipe.Game.Effects.Popped;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game.Floors
{
    public class StartingFloor : Floor
    {
        private readonly int _level;
        public StartingFloor(SKCanvas canvas, float x, float y, float height, float width, int level) : base(canvas, x, y, height, width)
        {
            _level = level;

            //AddChild(new TextBlock(Canvas, Width / 2, Height * .05f, $"Level", (int)Width / 10, new SKColor(255, 255, 255, 255)));
            //AddChild(new TextBlock(Canvas, Width / 2, Height * .2f, _level.ToString(), (int)Width / 5, new SKColor(255, 255, 255, 255)));
        }

        public async Task Start()
        {
            var goText = new PoppedText(Canvas, Width / 2, Height / 2, 100, 100, 300, "Go !", 0.2f * Height, new SKColor(255, 255, 255, 255));
            AddChild(goText);

            await goText.Pop();
        }

        protected override void Draw()
        {
        }
    }
}
