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
using GemSwipe.Paladin.UIElements.Buttons;
using Newtonsoft.Json;

namespace GemSwipe.Game.Test
{
    class WorldTester : SkiaView
    {
        private int _unlockedLevelId;
        private IWorld _world;
        public WorldTester()
        {

            _unlockedLevelId = 1;
            PlayerDataService.Instance.SetMaxLevelReached(_unlockedLevelId);
            PlayerDataService.Instance.SaveChanges();

            _world = new SecondWorld();

            AddChild(_world);

            var popButton = new TextButton(Width / 2, 9.5f * Height / 10, Height / 40, "Pop !");
            AddChild(popButton);

            popButton.Activated += PopButton_Activated;


        }

        private void PopButton_Activated()
        {
            _world.Advance(new LevelClearedResult() { ClearedLevelId = _unlockedLevelId, NextLevelId = _unlockedLevelId + 1 });
            _unlockedLevelId++;
        }


        protected override void Draw()
        {
        }
    }
}
