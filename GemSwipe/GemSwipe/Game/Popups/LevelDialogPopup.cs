using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Popups
{
    public class LevelDialogPopup : DialogPopup
    {
        public LevelDialogPopup(int levelId) : base(false)
        {
            Popup.Title = $"Mission {levelId}";
            Popup.ActionName = "Let's Go !";
        }
    }
}
