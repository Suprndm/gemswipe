using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Gestures;
using GemSwipe.Game.Models;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Navigation.Pages;
using GemSwipe.Game.Settings;
using GemSwipe.Services;
using GemSwipe.Utilities;
using GemSwipe.Utilities.Buttons;
using GemSwipe.Utilities.Sprites;
using SkiaSharp;
using Xamarin.Forms;
using DependencyService = Xamarin.Forms.DependencyService;

namespace GemSwipe.Game.Pages.Map
{
    public class MapPage : PageBase
    {
        private float _verticalMargin;

        private Map _map;

        public MapPage(float x, float y, float height, float width) : base(x, y, height, width)
        {

            _verticalMargin = height / 10;

            _map = new Map(0, 0, Height, Width);

            AddChild(_map);
        }

        protected override void Draw()
        {
        }

        private void Pan(Point p)
        {
            _map.MoveToY((float)p.Y);
        }

        protected override async Task TransitionOut()
        {
            this.Animate("fadeOut", p => _opacity = (float)p, _opacity, 0f, 8, (uint)3000, Easing.CubicIn);
            await Task.Delay(1000);
        }

        protected override void OnActivated(object parameter = null)
        {
            Task.Run(async () =>
            {
                await Task.Delay(1000);
            });
        }
        protected override void OnDeactivated()
        {
        }
    }
}
