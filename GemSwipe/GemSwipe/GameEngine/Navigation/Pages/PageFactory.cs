using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.GameEngine.Navigation.Pages
{
    public class PageFactory
    {
        private readonly IDictionary<PageType, IPage> _pages;

        public PageFactory()
        {
            _pages = new Dictionary<PageType, IPage>();
        }

        public IPage GetPage(PageType pageType)
        {
            return  _pages[pageType];
        }
    }
}
