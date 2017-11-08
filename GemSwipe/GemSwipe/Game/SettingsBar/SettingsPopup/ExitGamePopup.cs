using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.UIElements.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.SettingsBar.SettingsPopup
{
    public class ExitGamePopup : DialogPopup
    {
        public ExitGamePopup() : base(SkiaRoot.ScreenWidth * 0.7f, SkiaRoot.ScreenHeight * 0.3f, false)
        {
            Popup.Title = $"Exit game?";
            Popup.ActionName = "Exit";
        }
    }
}
