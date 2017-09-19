using System;
using System.Threading.Tasks;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.GameEngine.Menu
{
    public class Background : SkiaView
    {
        private readonly OceanDepth _oceanDepth;

        public Background(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height,
            width)
        {
            _oceanDepth = new OceanDepth(Canvas, X, Y, Height, Width);
            AddChild(_oceanDepth);

            var blackHalo = new Halo(Canvas, X - Width / 3, Y, Height, Width * 3f, new SKColor(255, 255, 255), Math.PI);
            var whiteHalo = new Halo(Canvas, X - Width / 3, Y, Height, Width * 3f, new SKColor(0, 0, 0), 0);
            AddChild(blackHalo);
            AddChild(whiteHalo);
        }

        public Task OnNextBoard()
        {
           return _oceanDepth.ScrollDown();
        }

        protected override void Draw()
        {

        }
    }
}
