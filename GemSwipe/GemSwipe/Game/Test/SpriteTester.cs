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
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Sprites;
using GemSwipe.Paladin.UIElements.Buttons;
using Newtonsoft.Json;

namespace GemSwipe.Game.Test
{
    class SpriteTester : SkiaView
    {
        public SpriteTester()
        {
            var popButton = new TextButton(Width / 2, 9.5f * Height / 10, Height / 40, "Pop !");
            AddChild(popButton);

            popButton.Activated += PopButton_Activated;

            var sprite = new Sprite("shard",0,0,128,128);
            AddChild(sprite);
        }

        private void PopButton_Activated()
        {
         
        }

      

        protected override void Draw()
        {
        }
    }
}
