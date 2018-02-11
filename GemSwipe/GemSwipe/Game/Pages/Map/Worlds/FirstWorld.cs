using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Sprites;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSwipe.Game.Pages.Map.Worlds
{
    public class FirstWorld : WorldBase
    {
        public FirstWorld() : base(1)
        {
            var planetSprite = new Sprite(SpriteConst.Planet1, Width/2, Height*0.6f, Width*.786f, Height*.323f, new SKPaint { Color = new SKColor(255, 255, 255) });
            AddChild(planetSprite);
        }
    }
}
