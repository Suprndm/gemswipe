using System;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using GemSwipe.Droid.Effects;
using GemSwipe.Paladin.Gestures;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ResolutionGroupName("GemSwipe")]
[assembly: ExportEffect(typeof(TapWithPositionGestureEffect), nameof(TapWithPositionGestureEffect))]

namespace GemSwipe.Droid.Effects
{
    public class TapWithPositionGestureEffect : PlatformEffect
    {
        private GestureDetectorCompat gestureRecognizer;
        private readonly InternalGestureDetector tapDetector;

        private DisplayMetrics displayMetrics;

        public TapWithPositionGestureEffect()
        {
            tapDetector = new InternalGestureDetector
            {
                DownAction = motionEvent =>
                {
                    var x = motionEvent.GetX();
                    var y = motionEvent.GetY();

                    var point = new Point(x, y);
                    Gesture.OnDown(point);
                },

                UpAction = motionEvent =>
                {

                    var x = motionEvent.GetX();
                    var y = motionEvent.GetY();

                    var point = new Point(x, y);
                    Gesture.OnUp(point);
                },
                SwipeAction = point =>
                {
                    Gesture.OnSwipe(point);
                }
            };
        }

        protected override void OnAttached()
        {
            var control = Control ?? Container;

            var context = control.Context;
            displayMetrics = context.Resources.DisplayMetrics;
            tapDetector.Density = displayMetrics.Density;

            if (gestureRecognizer == null)
                gestureRecognizer = new GestureDetectorCompat(context, tapDetector);
            control.Touch += ControlOnTouch;

        }

        private void ControlOnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            if (touchEventArgs.Event.Action == MotionEventActions.Move)
            {
                Gesture.OnPan(new Point(touchEventArgs.Event.GetX(), touchEventArgs.Event.GetY()));
            }
            if (touchEventArgs.Event.Action == MotionEventActions.Up)
            {
                Gesture.OnUp(new Point(touchEventArgs.Event.GetX(), touchEventArgs.Event.GetY()));
            }
                gestureRecognizer?.OnTouchEvent(touchEventArgs.Event);
        }

        protected override void OnDetached()
        {
            var control = Control ?? Container;
            control.Touch -= ControlOnTouch;
        }


        sealed class InternalGestureDetector : GestureDetector.SimpleOnGestureListener
        {
            public Action<MotionEvent> DownAction { get; set; }
            public Action<MotionEvent> UpAction { get; set; }
            public Action<Point> SwipeAction { get; set; }
            public float Density { get; set; }

            public override bool OnDown(MotionEvent e)
            {
                DownAction?.Invoke(e);
                return base.OnDown(e);
            }

            public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
            {
                SwipeAction?.Invoke(new Point(-distanceX, -distanceY));
                return base.OnScroll(e1, e2, distanceX, distanceY);
            }
        }
    }
}
