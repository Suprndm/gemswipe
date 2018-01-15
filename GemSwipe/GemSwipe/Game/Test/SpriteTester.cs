using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Events;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Popups;
using GemSwipe.Game.Shards;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Sprites;
using GemSwipe.Paladin.UIElements.Buttons;
using Newtonsoft.Json;
using SkiaSharp;

namespace GemSwipe.Game.Test
{
    class SpriteTester : SkiaView
    {
        public SpriteTester()
        {
            var popButton = new TextButton(Width / 2, 9.5f * Height / 10, Height / 40, "Pop !");
            AddChild(popButton);

            popButton.Activated += PopButton_Activated;


        }

        private void PopButton_Activated()
        {

            var sprite = new Sprite(SpriteConst.WhiteHalo, Width / 2, Height / 2, 4096, 4096, new SKPaint { Color = CreateColor(255, 255, 255,100), BlendMode = SKBlendMode.Plus });
            AddChild(sprite);

        }

      

        protected override void Draw()
        {
        }
    }
}
