using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.Navigation.Pages;

namespace GemSwipe.Game.Pages.Home
{
    public class HomePage: PageBase
    {
        public HomePage() : base()
        {
            var tapToPlay = new TapToPlay();
            AddChild(tapToPlay);
            tapToPlay.Down += OnTapped;
        }

        private void OnTapped()
        {
            Navigator.Instance.GoTo(PageType.Map);
        }

        protected override void Draw()
        {
         
        }

        protected override void OnActivated(object parameter = null)
        {

        }

        protected override void OnDeactivated()
        {
        }
    }
}
