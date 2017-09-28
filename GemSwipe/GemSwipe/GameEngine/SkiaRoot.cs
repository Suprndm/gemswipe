using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.BoardSolver;
using GemSwipe.Data;
using GemSwipe.GameEngine.Effects;
using GemSwipe.GameEngine.Game;
using GemSwipe.GameEngine.Home;
using GemSwipe.GameEngine.Map;
using GemSwipe.GameEngine.Menu;
using GemSwipe.GameEngine.Navigation;
using GemSwipe.GameEngine.Popped;
using GemSwipe.GameEngine.SkiaEngine;
using GemSwipe.Models;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.GameEngine
{
    public class SkiaRoot : SkiaView
    {
        
        private readonly Background _background;

        public SkiaRoot(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            // RegisterPages 
            var homePage = new HomePage(canvas, 0, 0, height, width);
            AddChild(homePage);
            var mapPage = new MapPage(canvas, 0, 0, height, width);
            AddChild(mapPage);
            var gamePage = new GamePage(canvas, 0, 0, height, width);
            AddChild(gamePage);
            Navigator.Instance.RegisterPage(PageType.Home, homePage);
            Navigator.Instance.RegisterPage(PageType.Map, mapPage);
            Navigator.Instance.RegisterPage(PageType.Game, gamePage);
            Navigator.Instance.GoToInitialPage(PageType.Home);


            _background = new Background(canvas, 0, 0, height, width);
            AddChild(_background, -1);
        }

        protected override void Draw()
        {

        }


        public override void Dispose()
        {
        }
    }
}