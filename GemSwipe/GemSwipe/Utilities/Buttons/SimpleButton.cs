using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Utilities
{

    public class SimpleButton : SkiaView
    {
        public SKColor Color { get; set; }
        protected SKRect _hitbox;
        private Action _onTappedHandler;

        public SimpleButton( float x, float y, float width, float height, SKColor color) : base( x, y, height, width)
        {
            Color = color;
            DeclareTappable(this);
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
              
                paint.IsAntialias = true;
                paint.Color = CreateColor(Color);

                _hitbox = SKRect.Create(X-Width/2, Y-Height/2, Width, Height);
                //Canvas.DrawText(Text, X - textLenght / 2, Y + Size / 2, paint);
                Canvas.DrawRect(_hitbox, paint);

            }
        }

        public override SKRect GetHitbox()
        {
            return _hitbox;
        }

        public void OnTapped_Action(Action OnTapped)
        {
            Tapped -= _onTappedHandler;
            _onTappedHandler = OnTapped;
            Tapped+= _onTappedHandler;
        }

        public override void Dispose()
        {
            Tapped -= _onTappedHandler;
            base.Dispose();
        }
    }
}
