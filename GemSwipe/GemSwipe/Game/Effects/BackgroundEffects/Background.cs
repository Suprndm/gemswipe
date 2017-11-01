using System;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Navigation;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class Background : SkiaView
    {
        private readonly OceanDepth _oceanDepth;
        private readonly Stars _stars;

        public Background()
        {
            _oceanDepth = new OceanDepth( X, Y, Height, Width);
            AddChild(_oceanDepth);

            var blackHalo = new Halo( X - Width / 3, Y, Height, Width * 3f, CreateColor(255, 255, 255), Math.PI);
            var whiteHalo = new Halo( X - Width / 3, Y, Height, Width * 3f, CreateColor(0, 0, 0), 0);
            AddChild(blackHalo);
            AddChild(whiteHalo);

            //_stars = new Stars( X, Y, Height, Width);
            //AddChild(_stars);
        }

        public Task OnNextBoard()
        {
            //_stars.ScrollDown();
            return _oceanDepth.ScrollDown();
        }

        public Task PlayTransition(PageType currentPage, PageType nextPage)
        {
            return Task.Delay(1000);
        }

        public void OnNavigateTo(PageType pageType)
        {
            //switch (pageType)
            //{
            //    case PageType.Home:
            //        _stars.SetAcceleration(1);
            //        break;
            //    case PageType.Map:
            //        _stars.SetAcceleration(5);
            //        break;
            //    case PageType.Settings:
            //        break;
            //    case PageType.Game:
            //        _stars.SetAcceleration(0.1f);
            //        break;
            //}
        }

        public void EndTransition()
        {
            //_stars.SetAcceleration(0.1f);
        }

        protected override void Draw()
        {

        }
    }
}
