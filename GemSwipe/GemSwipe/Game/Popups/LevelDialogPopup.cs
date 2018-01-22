using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Paladin.Containers;
using GemSwipe.Paladin.UIElements;
using GemSwipe.Paladin.UIElements.Popups;
using SkiaSharp;

namespace GemSwipe.Game.Popups
{
    public class LevelDialogPopup : DialogPopup
    {
        public LevelDialogPopup(LevelData levelData) : base(SkiaRoot.ScreenWidth * 0.7f, SkiaRoot.ScreenHeight * 0.3f, false)
        {
            Popup.Title = $"Mission {levelData.Id}";
            Popup.ActionName = "Let's Go !";


            var container = new Container();


            Popup.AddContent(container);
        }
    }
}
