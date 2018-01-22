using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Containers;
using GemSwipe.Paladin.UIElements;
using GemSwipe.Paladin.UIElements.Popups;

namespace GemSwipe.Game.Popups
{
    public class OutOfLifePopup : DialogPopup
    {
        public OutOfLifePopup() : base(SkiaRoot.ScreenWidth * 0.7f, SkiaRoot.ScreenHeight * 0.3f, false)
        {
            Popup.Title = $"Oups...";
            Popup.ActionName = "Refill";

            var container = new Container();
            container.AddContent(new TextBlock(0, 0, "Seems like you are out of life", SkiaRoot.ScreenHeight * 0.03f, CreateColor(255, 255, 255)));

            Popup.AddContent(container);
        }
    }
}
