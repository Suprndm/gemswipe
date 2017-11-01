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
using GemSwipe.Paladin.UIElements.Buttons;
using Newtonsoft.Json;

namespace GemSwipe.Game.Test
{
    class ShardTester : SkiaView
    {
        public ShardTester()
        {
            var popButton = new TextButton(Width / 2, 9.5f * Height / 10, Height / 40, "Pop !");
            AddChild(popButton);

            popButton.Activated += PopButton_Activated;
        }

        private void PopButton_Activated()
        {
            var shard = new Shard(Width/2, Height/2, Width/7, Width/7);
            AddChild(shard);

            shard.Down += () =>
            {
                shard.Die();
            };
        }

      

        protected override void Draw()
        {
        }
    }
}
