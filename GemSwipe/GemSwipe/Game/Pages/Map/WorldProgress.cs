using GemSwipe.Paladin.Core;
using SkiaSharp;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GemSwipe.Game.Pages.Map
{
    public class WorldProgress : SkiaView
    {
        private readonly float _initialY;
        private float _targetY;
        private const int AnimationMs = 1000;

        public WorldProgress(float initialY, float targetY)
        {
            _initialY = initialY;
            _targetY = targetY;
        }

        protected override void Draw()
        {
            if (_initialY == _targetY) return;

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.StrokeWidth = Height / 200;
                paint.Color = CreateColor(255, 255, 255, 255);
                Canvas.DrawLine(X + Width / 2, Y + _initialY,X + Width / 2, Y + _targetY, paint);
            }
        }

        public Task AdvanceTo(float targetY)
        {
            this.Animate("worldprogressY", p => _targetY = (float)p, _targetY, targetY, 4, AnimationMs, Easing.CubicInOut);
            return Task.Delay(AnimationMs);
        }
    }
}
