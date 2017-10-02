using System;
using System.Threading.Tasks;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Navigation.Pages
{
    public abstract class PageBase : SkiaView, IPage
    {
        protected bool IsActive { get; private set; }
        protected PageBase(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            IsVisible = false;
        }

        protected abstract void OnActivated(object parameter = null);

        protected abstract void OnDeactivated();

        public PageType Type { get; set; }

        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }

        public async Task Show(object parameter = null)
        {
            OnActivated(parameter);
            IsVisible = true;

            // Fade
            await Task.Delay(1000);
        }

        public async Task Hide()
        {
            OnDeactivated();

            // Fade
            await Task.Delay(1000);
            IsVisible = false;
        }
    }
}
