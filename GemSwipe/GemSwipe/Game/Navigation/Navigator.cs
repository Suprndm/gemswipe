using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.Game.Navigation.Pages;

namespace GemSwipe.Game.Navigation
{
    public class Navigator
    {
        private readonly IDictionary<PageType, IPage> _pages;
        private IPage _currentPage;
        private static Navigator _instance;

        public static event Action<NavigationEventArgs> NavigationStarted;
        public static event Action<NavigationEventArgs> NavigationEnded;

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

        public void RegisterPage(PageType pageType, IPage page)
        {
            _pages.Add(pageType, page);
        }

        public Task GoToInitialPage(PageType pageType)
        {
            _currentPage = _pages[pageType];
            return _currentPage.Show();
        }

        public async Task GoTo(PageType nextPageType, object parameter = null)
        {
            var nextPage = _pages[nextPageType];

            NavigationStarted?.Invoke(new NavigationEventArgs(_currentPage.Type, nextPageType));

            await _currentPage.Hide();

            _currentPage = nextPage;
            await nextPage.Show(parameter);
            NavigationEnded?.Invoke(new NavigationEventArgs(_currentPage.Type, nextPageType));
        }
    }
}
