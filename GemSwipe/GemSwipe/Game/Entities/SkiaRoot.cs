﻿using System.Threading.Tasks;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Pages.Home;
using GemSwipe.Game.Pages.Map;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities.Sprites;
using SkiaSharp;

namespace GemSwipe.Game.Entities
{
    public class SkiaRoot : SkiaView
    {
        
        private Background _background;

        public SkiaRoot(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            Initialize();

          
        }
        public async void Initialize()
        {
            await LoadResources();
            SetupNavigation();
        }

        public async Task LoadResources()
        {
            SpriteSheet.Instance.Setup("Resources/Graphics/atlas.png", "Resources/Graphics/atlas.txt");
            await SpriteSheet.Instance.LoadAsync();
        }

        public void SetupNavigation()
        {
            var homePage = new HomePage(Canvas, 0, 0, Height, Width);
            AddChild(homePage);

            _background = new Background(Canvas, 0, 0, Height, Width);
            AddChild(_background, -1);

            var mapPage = new MapPage(Canvas, 0, 0, Height, Width);
            AddChild(mapPage);
            var gamePage = new GamePage(Canvas, 0, 0, Height, Width);
            AddChild(gamePage);

            Navigator.Instance.SetBackground(_background);
            Navigator.Instance.RegisterPage(PageType.Home, homePage);
            Navigator.Instance.RegisterPage(PageType.Map, mapPage);
            Navigator.Instance.RegisterPage(PageType.Game, gamePage);
            Navigator.Instance.GoToInitialPage(PageType.Home);
        }

        protected override void Draw()
        {

        }


        public override void Dispose()
        {
        }
    }
}