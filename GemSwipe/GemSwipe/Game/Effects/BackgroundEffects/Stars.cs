using System;
using System.Collections.Generic;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using Xamarin.Forms.Internals;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class Stars : SkiaView
    {
        private IList<Star> _stars;
        private Random _randomizer;
        public Stars( float x, float y, float height, float width) : base( x, y, height, width)
        {
            _stars= new List<Star>();
            _randomizer = new Random();
            for (int i = 0; i < 100; i++)
            {
                var star = new Star( _randomizer, height, width);
                _stars.Add(star);
                AddChild(star);

            }
        }

        protected override void Draw()
        {
        }


        public void SetAcceleration(float factor)
        {
            foreach (var star in _stars)
            {
                star.SetAcceleration(factor);
            }
        }

        public void ScrollDown()
        {
            foreach (var star in _stars)
            {
                star.Slide(0);
            }
        }
    }
}
