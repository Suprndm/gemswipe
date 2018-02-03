using System;
using System.Linq;
using GemSwipe.Game.Models;
using Xamarin.Forms;

namespace GemSwipe.Paladin.Gestures
{
    public static class Gesture
    {
        public static event Action<Point> Pan;
        public static event Action<Point,Direction> Swipe;
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

            _previousPosX = p.X;
            _previousPosY = p.Y;

            Pan?.Invoke(new Point(dragX, dragY));

            OnSwipe(new Point(dragX, dragY));
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
                var initialPos = new Point(_previousPosX, _previousPosY);
                if (eX > 0)
                {
                    if (eY > eX)
                        Swipe?.Invoke(initialPos, Direction.Bottom);
                    else if (Math.Abs(eY) > eX)
                        Swipe?.Invoke(initialPos, Direction.Top);
                    else
                        Swipe?.Invoke(initialPos, Direction.Right);
                }
                else
                {
                    if (eY > Math.Abs(eX))
                        Swipe?.Invoke(initialPos, Direction.Bottom);
                    else if (Math.Abs(eY) > Math.Abs(eX))
                        Swipe?.Invoke(initialPos, Direction.Top);
                    else
                        Swipe?.Invoke(initialPos, Direction.Left);
                }
            }
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
    }
}