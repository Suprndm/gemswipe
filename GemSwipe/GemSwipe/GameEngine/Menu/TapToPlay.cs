using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.GameEngine.Menu
{
    public class TapToPlay : TextBlock
    {
        private double _angle;
        private Animation _animation;
        public TapToPlay(SKCanvas canvas, float x, float y, float width, float height) : base(canvas, x, y, "Tap to play", height/20, SKColor.Empty)
        {
            Width = width;
            Height = height;

            _animation = new Animation(p => _angle = p, 0, 179);
            _animation.Commit(this, "tapToPlay", 16, 2000, Easing.Linear, repeat: () => true);

            DeclareTappable(this);
            Tapped += TapToPlay_Tapped;
        }

        private void TapToPlay_Tapped()
        {
            this.AbortAnimation("tapToPlay");
            Dispose();
        }

        protected override void Draw()
        {
            byte opacity = (byte) (Math.Sin(Math.PI * _angle / 180) * 255);
            Color = new SKColor(255, 255, 255, opacity);
            base.Draw();
        }
    }
}
