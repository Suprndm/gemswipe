using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Navigation.Pages;
using GemSwipe.Game.Settings;

namespace GemSwipe.Game.Navigation
{
    public class Navigator
    {
        private readonly IDictionary<PageType, IPage> _pages;

        private IPage _currentPage;
        private Background _background;
        private SettingsPanel _settingsPanel;
        private static Navigator _instance;

        private Navigator()
        {
            _pages = new Dictionary<PageType, IPage>();

        }

        public static Navigator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Navigator();
                }
                return _instance;
            }
        }

        public void SetBackground(Background background)
        {
            _background = background;
        }

        public void SetSettingsPanel(SettingsPanel settingsPanel)
        {
            _settingsPanel = settingsPanel;
        }

        public void RegisterPage(PageType pageType, IPage page)
        {
            _pages.Add(pageType, page);
        }


        public Task GoToInitialPage(PageType pageType)
        {
            _currentPage = _pages[pageType];
            _background.OnNavigateTo(pageType);
            return _currentPage.Show();
        }

        public async Task GoTo(PageType pageType, object parameter = null)
        {
            var nextPage = _pages[pageType];

            await _currentPage.Hide();

            _currentPage = nextPage;
            _background.OnNavigateTo(pageType);
            await nextPage.Show(parameter);
            _background.EndTransition();
        }

        public async Task ShowSettings()
        {
           await _settingsPanel.Show();
        }

        public Task HideSettings()
        {
            return _settingsPanel.Hide();
        }
    }
}
