﻿using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Navigation;
using Xamarin.Forms;
using Rectangle = GemSwipe.Paladin.Geometry.Rectangle;

namespace GemSwipe.Paladin.Layers
{
    public class LoadingLayer : SkiaView
    {
        private Rectangle _transitionScreen;

        public LoadingLayer()
        {
            _transitionScreen = new Rectangle(0, 0, Height, Width, CreateColor(250, 247, 240));

            AddChild(_transitionScreen);

            Navigator.NavigationStarted += Navigator_NavigationStarted;
            Navigator.InitialNavigationStarted += Navigator_InitialNavigationStarted;
        }

        private void Navigator_InitialNavigationStarted()
        {
            this.Animate("whiteSmokeOut", p => _transitionScreen.Opacity = (float)p, _transitionScreen.Opacity, 0, 8, (uint)2000, Easing.Linear);
        }

        private async void Navigator_NavigationStarted(Navigation.NavigationEventArgs arg)
        {
            this.Animate("whiteSmokeIn", p => _transitionScreen.Opacity = (float)p, _transitionScreen.Opacity, 1, 8, (uint)1000, Easing.Linear);
            await Task.Delay(1500);
            this.Animate("whiteSmokeOut", p => _transitionScreen.Opacity = (float)p, _transitionScreen.Opacity, 0, 8, (uint)1000, Easing.Linear);
        }

        protected override void Draw()
        {

        }
    }
}