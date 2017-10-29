using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.UIElements.Popups;

namespace GemSwipe.Game.Popups
{
    public class WinDialogPopup : DialogPopup
    {
        public WinDialogPopup() : base(SkiaRoot.ScreenWidth * 0.7f, SkiaRoot.ScreenHeight * 0.3f, false)
        {
            Popup.Title = $"Congratulation !";
            Popup.ActionName = "Next level";
        }
    }
}
