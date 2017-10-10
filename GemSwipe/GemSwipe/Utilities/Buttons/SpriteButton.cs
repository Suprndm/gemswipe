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
    public class SpriteButton : SimpleButton
    {
        public Sprite _sprite { get; set; }

        public SpriteButton( string name, float x, float y, float width, float height) : base( x, y, height, width, new SKColor(255, 255, 255))
        {
            _sprite = new Sprite( name, 0, 0, width, height);
            AddChild(_sprite);
        }

        protected override void Draw()
        {
            
        }
    }
}
