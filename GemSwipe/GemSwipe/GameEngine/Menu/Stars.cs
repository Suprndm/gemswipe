using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Menu
{
    public class Stars : SkiaView
    {
        private IList<Star> _stars;
        private Random _randomizer;
        public Stars(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _stars= new List<Star>();
            _randomizer = new Random();
            for (int i = 0; i < 100; i++)
            {
                var star = new Star(Canvas, _randomizer, height, width);
                //var star = new Star(Canvas, _randomizer.Next((int)Width), _randomizer.Next((int)Height), height, width, _randomizer.Next(1, 7), _randomizer.Next(10) / 100f, _randomizer.Next(400) / 100);
                _stars.Add(star);
                AddChild(star);
            }
        }

        protected override void Draw()
        {

        }

        public void ScrollDown()
        {
            foreach (var star in _stars)
            {
                star.Slide();
            }
        }
    }
}
