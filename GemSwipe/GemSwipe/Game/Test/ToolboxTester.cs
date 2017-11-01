using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Events;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Popups;
using GemSwipe.Game.Shards;
using GemSwipe.Game.Toolbox;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.UIElements.Buttons;
using Newtonsoft.Json;

namespace GemSwipe.Game.Test
{
    class ToolboxTester : SkiaView
    {
        public ToolboxTester()
        {
            var toolboxBar = new ToolboxBar(0, SkiaRoot.ScreenHeight * 0.85f, SkiaRoot.ScreenHeight * 0.15f, SkiaRoot.ScreenWidth);
            AddChild(toolboxBar);
        }




        protected override void Draw()
        {
        }
    }
}
