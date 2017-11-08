using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.Paladin.Navigation.Pages;

namespace GemSwipe.Paladin.Navigation
{
    public class Navigator
    {
        private readonly IDictionary<PageType, IPage> _pages;
        private IPage _currentPage;
        private static Navigator _instance;

        public static event Action<NavigationEventArgs> NavigationStarted;
        public static event Action<NavigationEventArgs> NavigationEnded;
        public static event Action InitialNavigationStarted;

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

        public async Task GoToInitialPage(PageType pageType)
        {
            _currentPage = _pages[pageType];
            InitialNavigationStarted?.Invoke();
            await _currentPage.Show();
          //  NavigationEnded?.Invoke(new NavigationEventArgs(pageType, pageType));
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

        public IPage GetCurrentPage()
        {
            return _currentPage;
        }
    }
}
