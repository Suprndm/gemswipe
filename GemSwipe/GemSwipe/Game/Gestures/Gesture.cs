using System;
using System.Linq;
using GemSwipe.Game.Models;
using Xamarin.Forms;

namespace GemSwipe.Game.Gestures
{
    public static class Gesture
    {
        public static event Action<Point> Pan;
        public static event Action<Direction> Swipe;
        public static event Action<Point> Up;
        public static event Action<Point> Down;
        private static bool _panJustBegun;
        private static double _dragX;
        private static double _dragY;
        private static double _previousPosX;
        private static double _previousPosY;

        public static void OnPan(Point p)
        {
            var dragX = p.X - _previousPosX;
            var dragY = p.Y - _previousPosY;
            Pan?.Invoke(new Point(dragX, dragY));

            _previousPosX = p.X;
            _previousPosY = p.Y;
        }

        public static void OnSwipe(Point p)
        {

            if (p.X == 0 && p.Y == 0) return;

            var eX = p.X;
            var eY = p.Y;
            var d = Math.Sqrt(eX * eX + eY * eY);

            if (d > 25 && _panJustBegun)
            {
                _panJustBegun = false;
                if (eX > 0)
                {
                    if (eY > eX)
                        Swipe?.Invoke(Direction.Bottom);
                    else if (Math.Abs(eY) > eX)
                        Swipe?.Invoke(Direction.Top);
                    else
                        Swipe?.Invoke(Direction.Right);
                }
                else
                {
                    if (eY > Math.Abs(eX))
                        Swipe?.Invoke(Direction.Bottom);
                    else if (Math.Abs(eY) > Math.Abs(eX))
                        Swipe?.Invoke(Direction.Top);
                    else
                        Swipe?.Invoke(Direction.Left);
                }
            }
        }

        public static void Setup(View view)
        {
            var effect = GetOrCreateEffect(view);
        }

        public static void OnUp(Point p)
        {
            Up?.Invoke(p);
            _panJustBegun = false;
        }

        public static void OnDown(Point p)
        {
            _previousPosX = p.X;
            _previousPosY = p.Y;

            _panJustBegun = true;
            Down?.Invoke(p);
        }

        private static GestureEffect GetOrCreateEffect(View view)
        {
            var effect = (GestureEffect)view.Effects.FirstOrDefault(e => e is GestureEffect);
            if (effect == null)
            {
                effect = new GestureEffect();
                view.Effects.Add(effect);
            }
            return effect;
        }

        class GestureEffect : RoutingEffect
        {
            public GestureEffect() : base("GemSwipe.TapWithPositionGestureEffect")
            {
            }
        }
    }
}