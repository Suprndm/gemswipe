using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Events
{
    public class EventCounter:SkiaView
    {
        private TextBlock _counterText;
        public EventCounter(float x, float y, float height, float width) : base(x, y, height, width)
        {
            _counterText = new TextBlock(Width/2, height/2,"", Height/3, new SKColor(255,255,255));
            AddChild(_counterText);
        }

        protected override void Draw()
        {

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = CreateColor(125,125,125);
                Canvas.DrawRect(SKRect.Create(X,Y,Width, Height),  paint);
            }
        }

        public void UpdateCount(int count)
        {
            _counterText.Text = count.ToString();
        }
    }
}
