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
using GemSwipe.Paladin.UIElements;
using GemSwipe.Paladin.UIElements.Buttons;
using Newtonsoft.Json;
using SkiaSharp;

namespace GemSwipe.Game.Test
{
    class ShardTester : SkiaView
    {
        private TextBlock _countText;
        private int _count;
        public ShardTester()
        {
            var popButton = new TextButton(Width / 2, 9.5f * Height / 10, Height / 40, "Pop !");
            AddChild(popButton);

            var textblock = new TextBlock(Width / 2, 7.5f * Height / 10, "0", Height / 20, new SKColor(255,255,255) );
            _countText = textblock;
            AddChild(_countText);
            popButton.Activated += PopButton_Activated;
            var count = 0;
            for (int i = 0; i < 1; i++)
            {
                Task.Run(() =>
                {
                    var shard = new Shard(Width / 2, Height / 2, Width / 7, Width / 7);
                    AddChild(shard);
                    _count++;
                });


            }
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
            _countText.Text = _count.ToString();

        }
    }
}
