using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Behaviors;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.Navigation.Pages;
using GemSwipe.Paladin.Sprites;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Home
{
    public class HomePage : PageBase
    {
        private Sprite _logoSprite;
        private Sprite _bigPlanetSprite;
        private Sprite _mediumPlanetSprite;
        private Sprite _smallPlanetSprite;
        public HomePage()
        {
            Type = PageType.Home;

            var tapToPlay = new TapToPlay();
            AddChild(tapToPlay);
            tapToPlay.Down += OnTapped;

            _logoSprite = new Sprite(SpriteConst.BelowTheStars, Width / 2, Height * 0.2f, Width * 0.33f, Height * 0.2f, new SKPaint { Color = new SKColor(255, 255, 255), BlendMode = SKBlendMode.Plus });
            _logoSprite.Opacity = 0.7f;
            _logoSprite.AddBehavior(new ToastBehavior(0, 0.3f, 1, 5000, 5000));

            _smallPlanetSprite = new Sprite(SpriteConst.SmallPlanet, Width /2, Height * 0.7f, Width * 0.37f, Width * 0.37f, new SKPaint { Color = new SKColor(255, 255, 255) , BlendMode = SKBlendMode.Plus });
            _smallPlanetSprite.Opacity = 0.7f;
            _smallPlanetSprite.AddBehavior(new ToastBehavior(0, 0.2f, 1, 10000, 15000));

            _mediumPlanetSprite = new Sprite(SpriteConst.MediumPlanet, Width / 2, Height * 0.5f, Width * 0.55f, Width * 0.5f, new SKPaint { Color = new SKColor(255, 255, 255), BlendMode = SKBlendMode.Plus });
            _mediumPlanetSprite.Opacity = 0.2f;
            _mediumPlanetSprite.AddBehavior(new ToastBehavior(0, 0, 1, 0, 12000));

            _bigPlanetSprite = new Sprite(SpriteConst.MediumPlanet, Width / 2, Height * 0.2f, Width * 0.78f, Width * 0.78f, new SKPaint { Color = new SKColor(255, 255, 255), BlendMode = SKBlendMode.Plus });
            _bigPlanetSprite.Opacity = 0.1f;
            _bigPlanetSprite.AddBehavior(new ToastBehavior(0, -0.3f, 1, 10000, 4000));

            AddChild(_smallPlanetSprite);


            AddChild(_mediumPlanetSprite);


            AddChild(_bigPlanetSprite);


            AddChild(_logoSprite);
        }

        private void OnTapped()
        {
            Navigator.Instance.GoTo(PageType.Map);
        }

        protected override void Draw()
        {
            float rotationSpeed = 0.001f;
            _smallPlanetSprite.Angle += rotationSpeed;
            _mediumPlanetSprite.Angle += rotationSpeed;
            _bigPlanetSprite.Angle += rotationSpeed;
        }

        protected override void OnActivated(object parameter = null)
        {

        }

        protected override void OnDeactivated()
        {
        }
    }
}
