using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.UIElements.Popups;

namespace GemSwipe.Game.SettingsBar.SettingsPopup
{
 
    public class RestartGamePopup : DialogPopup
    {
        public RestartGamePopup() : base(SkiaRoot.ScreenWidth * 0.7f, SkiaRoot.ScreenHeight * 0.3f, false)
        {
            Popup.Title = $"Restart game?";
            Popup.ActionName = "Restart";
        }
    }
}
