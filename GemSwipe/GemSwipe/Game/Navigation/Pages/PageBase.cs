using System;
using System.Threading.Tasks;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Navigation.Pages
{
    public abstract class PageBase : SkiaView, IPage
    {
        protected bool IsActive { get; private set; }
        protected PageBase( float x, float y, float height, float width) : base( x, y, height, width)
        {
            IsVisible = false;
            IsEnabled = false;
            _opacity = 0;
        }

        protected abstract void OnActivated(object parameter = null);

        protected abstract void OnDeactivated();

        public PageType Type { get; set; }

        public virtual void Initialize()
        {
        }

        public async Task Show(object parameter = null)
        {
            OnActivated(parameter);
            IsVisible = true;
            IsEnabled = true;
            await TransitionIn();
        }

        protected virtual async Task TransitionIn()
        {
            this.Animate("fadeIn", p => _opacity = (float)p, _opacity, 1f, 8, (uint)300, Easing.CubicIn);
            await Task.Delay(300);
        }

        protected virtual async Task TransitionOut()
        {
            this.Animate("fadeOut", p => _opacity = (float)p, _opacity, 0f, 8, (uint)300, Easing.CubicIn);
            await Task.Delay(300);
        }

        public async Task Hide()
        {
            await TransitionOut();
            IsVisible = false;
            IsEnabled = false;
            OnDeactivated();
        }
    }
}
