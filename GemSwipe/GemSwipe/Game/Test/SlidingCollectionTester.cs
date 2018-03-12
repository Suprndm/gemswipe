using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Data.PlayerData;
using GemSwipe.Game.Events;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Pages.Map;
using GemSwipe.Game.Pages.Map.Worlds;
using GemSwipe.Game.Popups;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Geometry;
using GemSwipe.Paladin.UIElements;
using GemSwipe.Paladin.UIElements.Buttons;
using Newtonsoft.Json;

namespace GemSwipe.Game.Test
{
    class SlidingCollectionTester : SkiaView
    {
        public SlidingCollectionTester()
        {
            var unlockedLevelId = 11;
            PlayerDataService.Instance.SetMaxLevelReached(unlockedLevelId);
            PlayerDataService.Instance.SaveChanges();


            AddChild(new SlidingCollection(0, 0, Width, Height, new List<ISkiaView>
                {
                new FirstWorld(),
                new SecondWorld(),
                new ThirdWorld(),
                new SecondWorld(),
                new ThirdWorld(),
                    //new Rectangle(0,0,Height/3, Width/3, CreateColor(255,255,255)),
                }
            ));
        }

        protected override void Draw()
        {
        }
    }
}
