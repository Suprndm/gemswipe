using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Popups;
using GemSwipe.Game.SkiaEngine;

namespace GemSwipe.Game.Layers
{
    public class PopupLayer : SkiaView
    {
        public PopupLayer()
        {
            PopupService.Instance.OnNewPopup += Instance_OnNewPopup;
        }

        private void Instance_OnNewPopup(IDialogPopup dialogPopup)
        {
            AddChild(dialogPopup);
        }

        protected override void Draw()
        {

        }
    }
}
