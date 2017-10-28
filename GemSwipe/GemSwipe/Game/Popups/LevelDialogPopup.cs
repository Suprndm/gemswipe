using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Containers;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Entities;
using GemSwipe.Game.Pages.Game;
using SkiaSharp;

namespace GemSwipe.Game.Popups
{
    public class LevelDialogPopup : DialogPopup
    {
        public LevelDialogPopup(LevelData levelData) : base(SkiaRoot.ScreenWidth * 0.7f, SkiaRoot.ScreenHeight * 0.3f, false)
        {
            Popup.Title = $"Mission {levelData.Id}";
            Popup.ActionName = "Let's Go !";

            var objectivesView = new ObjectivesView(levelData.Objectives, false, Width / 2, 0, 0.1f * Height);

            var objectiveText = new TextBlock(0, -ContentHeight * 0.2f, "Objectives:", SkiaRoot.ScreenHeight * 0.025f,
                new SKColor(255, 255, 255));

            var container = new Container();
            container.AddContent(objectiveText);
            Popup.AddContent(objectivesView);


            Popup.AddContent(container);
        }
    }
}
