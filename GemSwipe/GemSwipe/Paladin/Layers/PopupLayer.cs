using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.UIElements.Popups;

namespace GemSwipe.Paladin.Layers
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
