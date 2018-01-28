using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Effects;
using GemSwipe.Game.Events;
using GemSwipe.Game.Models.Entities;
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
    class StarEffectTester : SkiaView
    {
        private TextBlock _countText;
        private int _count;
        public StarEffectTester()
        {
            var popButton = new TextButton(Width / 2, 9.5f * Height / 10, Height / 40, "Pop !");
            AddChild(popButton);

            popButton.Activated += PopButton_Activated;
        }

        private void PopButton_Activated()
        {
            Task.Run( async () =>
            {
                var starEffect = new StarEffect(SkiaRoot.ScreenWidth / 2, SkiaRoot.ScreenHeight / 2);
                AddChild(starEffect);

                await starEffect.Start();

                await Task.Delay(2000);

                starEffect.Dispose();
            });

        }

      

        protected override void Draw()
        {

        }
    }
}
