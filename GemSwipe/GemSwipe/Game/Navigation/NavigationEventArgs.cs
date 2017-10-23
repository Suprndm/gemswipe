namespace GemSwipe.Game.Navigation
{
    public struct NavigationEventArgs
    {
        public NavigationEventArgs(PageType from, PageType to)
        {
            From = from;
            To = to;
        }

        public PageType From { get;}
        public PageType To { get; }
    }
}
