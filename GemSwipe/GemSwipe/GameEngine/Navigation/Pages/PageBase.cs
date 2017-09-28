using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Navigation.Pages
{
    public abstract class PageBase : SkiaView, IPage
    {
        protected bool IsActive { get; private set; }
        protected PageBase(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            IsVisible = false;
        }

        protected abstract void OnActivated();

        protected abstract void OnDeactivated();

        public PageType Type { get; set; }

        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }

        public async Task Show()
        {
            OnActivated();
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
