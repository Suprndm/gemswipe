using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities.Sprites;
using SkiaSharp;

namespace GemSwipe.Utilities.Buttons
{
    public class SpriteButton : SkiaView
    {
        public Sprite _sprite;

        public SpriteButton(SKCanvas canvas, string name, float x, float y, float width, float height) : base(canvas, x, y, width, height)
        {
            _sprite = new Sprite(canvas, name, x, y, width, 2*height);
            AddChild(_sprite);
        }

        protected override void Draw()
        {
            
        }
    }
}
