using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using Xamarin.Forms;

namespace GemSwipe.Paladin.Behaviors
{
    public class ToastBehavior:BehaviorBase
    {
        private float _initialX;
        private float _initialY;
        private float _initialOpacity;


        private float _animatedX;
        private float _animatedY;
        private float _animatedOpacity;

        private float _previousAnimatedX;
        private float _previousAnimatedY;
        private float _previousAnimatedOpacity;


        private readonly float _xToastRatio;
        private readonly float _yToastRatio;
        private readonly float _opacityToastRatio;
        private readonly uint _positionToastMs;
        private readonly uint _opacityToastMs;

        public ToastBehavior(float xToastRatio, float yToastRatio, float opacityToastRatio, uint positionToastMs = 400, uint opacityToastMs = 400)
        {
            _xToastRatio = xToastRatio;
            _yToastRatio = yToastRatio;
            _opacityToastRatio = opacityToastRatio;
            _positionToastMs = positionToastMs;
            _opacityToastMs = opacityToastMs;
        }

        public override void Attach(SkiaView skiaView)
        {
            var viewInitialX = skiaView.X - _xToastRatio * skiaView.X;
            var viewInitialY = skiaView.Y - _yToastRatio * skiaView.Y;
            var viewInitialOpacity = skiaView.Opacity - _opacityToastRatio * skiaView.Opacity;

            var viewTargetX = skiaView.X;
            var viewTargetY = skiaView.Y;
            var viewTargetOpacity = skiaView.Opacity;

            _initialX = viewInitialX - viewTargetX;
            _initialY = viewInitialY - viewTargetY;
            _initialOpacity = viewInitialOpacity - viewTargetOpacity;

            _previousAnimatedOpacity = _initialOpacity;
            _previousAnimatedX = _initialX;
            _previousAnimatedY = _initialY;

            skiaView.X += _initialX;
            skiaView.Y += _initialY;
            skiaView.Opacity += _initialOpacity;



            this.Animate("toastY", p => _animatedY = (float)p, _initialY, 0, 4, _positionToastMs, Easing.CubicOut);
            this.Animate("toastX", p => _animatedX = (float)p, _initialX, 0, 4, _positionToastMs, Easing.CubicOut);
            this.Animate("toastOpacity", p => _animatedOpacity = (float)p, _initialOpacity, 0, 4, _opacityToastMs, Easing.CubicOut);

            Task.Run(() =>
            {
                Task.Delay((int) Math.Max(_opacityToastMs, _positionToastMs)).Wait();
                Dispose();
            });

            base.Attach(skiaView);
        }

        public override void OnUpdated()
        {
            var diffX = _animatedX - _previousAnimatedX;
            var diffY = _animatedY - _previousAnimatedY;
            var diffOpacity = _animatedOpacity - _previousAnimatedOpacity;

            View.X += diffX;
            View.Y += diffY;
            View.Opacity += diffOpacity;

            _previousAnimatedX = _animatedX;
            _previousAnimatedY = _animatedY;
            _previousAnimatedOpacity = _animatedOpacity;
        }

        public override void Dispose()
        {
            this.AbortAnimation("toastX");
            this.AbortAnimation("toastY");
            this.AbortAnimation("toastOpacity");
        }
    }
}
