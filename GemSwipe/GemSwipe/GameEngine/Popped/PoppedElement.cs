using System.Threading.Tasks;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Popped
{
    public abstract class PoppedElement:SkiaView
    {
        protected readonly int DisplayDuration;
        protected readonly int AppearingDuration;
        protected readonly int DisappearingDuration;

        protected PoppedElement(SKCanvas canvas, float x, float y, float height, float width, int displayDuration, int appearingDuration, int disappearingDuration) : base(canvas, x, y, height, width)
        {
            DisplayDuration = displayDuration;
            AppearingDuration = appearingDuration;
            DisappearingDuration = disappearingDuration;
        }


        public async Task Pop()
        {
            await Appear();
            await Task.Delay(DisplayDuration);
            await Disappear();
            Dispose();
        }

        protected abstract Task Appear();

        protected abstract Task Disappear();

    }
}
