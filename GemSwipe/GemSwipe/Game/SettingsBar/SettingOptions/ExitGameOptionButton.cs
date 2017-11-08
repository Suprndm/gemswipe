using GemSwipe.Game.SettingsBar.SettingsPopup;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.UIElements.Popups;

namespace GemSwipe.Game.SettingsBar.SettingOptions
{
    public class ExitGameOptionButton : OptionButton
    {
        public ExitGameOptionButton(float x, float y, float radius) : base(x, y, radius)
        {

        }

        public override void OnActivated()
        {
            base.OnActivated();
            var exitGamePopup = new ExitGamePopup();
            PopupService.Instance.ShowPopup(exitGamePopup);
            exitGamePopup.NextCommand = () =>
            {
                Navigator.Instance.GoTo(PageType.Map);
            };
        }
    }
}
