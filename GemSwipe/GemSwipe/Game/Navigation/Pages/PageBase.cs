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
        protected PageBase() : base()
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
            _opacity = 1;
            //this.Animate("fadeIn", p => _opacity = (float)p, _opacity, 1f, 8, (uint)1000, Easing.CubicIn);
            await Task.Delay(1000);
        }

        protected virtual async Task TransitionOut()
        {
            //this.Animate("fadeOut", p => _opacity = (float)p, _opacity, 0f, 8, (uint)1000, Easing.CubicIn);
            await Task.Delay(1000);
            _opacity = 0;

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
