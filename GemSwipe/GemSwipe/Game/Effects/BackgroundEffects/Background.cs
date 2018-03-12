using System;
using System.Threading.Tasks;
using GemSwipe.Game.Sprites;
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
            _oceanDepth = new OceanDepth(X, Y, Height, Width);
            AddChild(_oceanDepth);

            var blackHalo = new Halo(SpriteConst.BlackHalo, X - Width / 3, Y, Height, Width * 3f, CreateColor(100, 100, 100), Math.PI);
            var whiteHalo = new Halo(SpriteConst.WhiteHalo ,X - Width / 3, Y, Height, Width * 3f, CreateColor(25, 25, 25), 0);
            AddChild(blackHalo);
            AddChild(whiteHalo);

            _stars = new Stars( X, Y, Height, Width);
            AddChild(_stars);
            Task.Run(async () =>
           {
               while (true)
               {
                   await _oceanDepth.ScrollDown();
               }
           });

            _stars.SetAcceleration(0.1f);

        }

        public void OnWorldChanged(int wolrdId)
        {
            _oceanDepth.OnWorldChanged(wolrdId);
        }

        public Task PlayTransition(PageType currentPage, PageType nextPage)
        {
            return Task.Delay(1000);
        }

        public void OnNavigateTo(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Home:
                    _oceanDepth.OnWorldChanged(0);
                    break;
                case PageType.Map:
                    _oceanDepth.OnWorldChanged(1);
                    break;
                case PageType.Settings:
                    break;
                case PageType.Game:
                    break;
            }
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
