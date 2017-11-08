using GemSwipe.Data.PlayerLife;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.SettingsBar.SettingsPopup;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.Navigation.Pages;
using GemSwipe.Paladin.UIElements.Popups;

namespace GemSwipe.Game.SettingsBar.SettingOptions
{
    public class RestartGameOptionButton : OptionButton
    {
        public RestartGameOptionButton(float x, float y, float radius) : base(x, y, radius)
        {

        }

        public override void OnActivated()
        {

            base.OnActivated();

            if (!PlayerLifeService.Instance.HasLife())
            {
                var noLifeLeft = new NoLifeLeftPopup();
                PopupService.Instance.ShowPopup(noLifeLeft);
                noLifeLeft.NextCommand = () =>
                {
                    Navigator.Instance.GoTo(PageType.Map);
                };
            }

            else
            {
                var restartGamePopup = new RestartGamePopup();
                PopupService.Instance.ShowPopup(restartGamePopup);
                restartGamePopup.NextCommand = () =>
                {
                    IPage currentPage = Navigator.Instance.GetCurrentPage();
                    if (currentPage.Type == PageType.Game)
                    {
                        GamePage currentGamePage = (GamePage)currentPage;
                        Navigator.Instance.GoTo(PageType.Game, currentGamePage.CurrentLevel);

                    }
                };
            }

        }

    }
}
