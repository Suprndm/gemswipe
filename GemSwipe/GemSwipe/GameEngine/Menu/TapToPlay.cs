using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.SkiaEngine;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.GameEngine.Menu
{
    public class TapToPlay : SkiaView
    {
        private readonly TextBlock _textBlock;
        private double _angle;

        public TapToPlay(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            Width = width;
            Height = height;

            _textBlock = new TextBlock(canvas, width / 2, height / 2, "Tap to play", height / 20, SKColor.Empty);
            AddChild(_textBlock);

            var animation = new Animation(p => _angle = p, 0, 179);
            animation.Commit(this, "tapToPlay", 16, 2000, Easing.Linear, repeat: () => true);

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
            _textBlock.Color = new SKColor(255, 255, 255, opacity);
        }
    }
}
