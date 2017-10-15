using System;
using System.Threading.Tasks;
using GemSwipe.Game.Gestures;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Services;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Utilities.Buttons
{
    public class SimpleButton : SkiaView, IButton
    {
        public event Action Activated;
        protected SKRect _hitbox;

        protected long DelayBetweenPresses = 300;

        protected SKColor NormalColor
        {
            get { return _normalColor; }
            set
            {
                _normalColor = value;
                R = NormalColor.Red;
                G = NormalColor.Green;
                B = NormalColor.Blue;
            }
        }

        protected SKColor DownColor = new SKColor(75, 75, 75);
        protected SKColor ActivatedColor = new SKColor(0, 168, 214);

        protected Byte R { get; set; }
        protected Byte G { get; set; }
        protected Byte B { get; set; }

        protected SKColor Color { get; set; }

        private bool _isDown;

        protected int TransitionMs = 300;
        private SKColor _normalColor;

        public SimpleButton(float x, float y, float width, float height) : base(x, y, height, width)
        {

            NormalColor = new SKColor(125, 125, 125);

            Gesture.Down += Gesture_Down;
            Gesture.Up += Gesture_Up;
        }

        private void Gesture_Up(Point p)
        {
            if (HitTheBox(p))
                OnUp();
            else if (_isDown)
            {
                _isDown = false;
                AnimateColorChange(NormalColor);
            }
        }

        private void Gesture_Down(Point p)
        {
            if (HitTheBox(p))
                OnDown();
        }

        public virtual async Task OnUp()
        {
            _isDown = false;
            if (CanActivate())
            {
                Activated?.Invoke();
                await AnimateColorChange(ActivatedColor);
                AnimateColorChange(NormalColor);

            }
            else
            {
                AnimateColorChange(NormalColor);
            }
        }

        public virtual void OnEnabled()
        {

        }

        public virtual void OnDown()
        {
            AnimateColorChange(DownColor);
            _isDown = true;
        }

        protected virtual Task AnimateColorChange(SKColor color)
        {
            this.Animate("colorR", p => R = (byte)p, R, color.Red, 4, (uint)TransitionMs, Easing.CubicOut);
            this.Animate("colorG", p => G = (byte)p, G, color.Green, 4, (uint)TransitionMs, Easing.CubicOut);
            this.Animate("colorB", p => B = (byte)p, B, color.Blue, 4, (uint)TransitionMs, Easing.CubicOut);

            return Task.Delay(TransitionMs);
        }

        protected override void Draw()
        {
            Color = new SKColor(R, G, B);
            using (var paint = new SKPaint())
            {

                paint.IsAntialias = true;
                paint.Color = CreateColor(Color);

                _hitbox = SKRect.Create(X - Width / 2, Y - Height / 2, Width, Height);
                Canvas.DrawRect(_hitbox, paint);
            }
        }

        protected virtual bool CanActivate()
        {
            return true;
        }


        public override SKRect GetHitbox()
        {
            return _hitbox;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
