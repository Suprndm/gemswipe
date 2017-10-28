using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Entities;

namespace GemSwipe.Game.Popups
{
    public class LoseDialogPopup : DialogPopup
    {
        public LoseDialogPopup() : base(SkiaRoot.ScreenWidth * 0.7f, SkiaRoot.ScreenHeight * 0.3f, false)
        {
            Popup.Title = $"Out of moves !";
            Popup.ActionName = "Try again !";
        }
    }
}
