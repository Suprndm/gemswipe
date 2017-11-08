using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.UIElements.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.SettingsBar.SettingsPopup
{
    public class NoLifeLeftPopup : DialogPopup
    {
        public NoLifeLeftPopup() : base(SkiaRoot.ScreenWidth * 0.7f, SkiaRoot.ScreenHeight * 0.3f, false)
        {
            Popup.Title = $"No Life left, continue?";
            Popup.ActionName = "Exit";
        }
    }
}
