using System.Collections.Generic;
using System.Linq;
using GemSwipe.Game.Models;
using GemSwipe.Paladin.Core;
using Xamarin.Forms;

namespace GemSwipe.Paladin.Gestures
{
    public class SkiaGestureService
    {
        private static SkiaGestureService _instance;
        private ISkiaView _downTappable;
        private Point _downPos;
        private Point _dragPos;
        private SkiaGestureService()
        {
        }

        public static SkiaGestureService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SkiaGestureService();
                }
                return _instance;
            }
        }

        public void HandleUp(IList<ISkiaView> tappables, Point p)
        {
            var tappedView = DetectInteractedTappable(tappables, p);
            if (tappedView != null && tappedView.IsEnabled)
            {
                tappedView.InvokeUp();
            }

            _downTappable = null;
        }

        public void HandleDown(IList<ISkiaView> tappables, Point p)
        {
            _downPos = p;

            var tappedView = DetectInteractedTappable(tappables, p);
            if (tappedView != null && tappedView.IsEnabled)
            {
                _downTappable = tappedView;
                _dragPos = new Point(p.X, p.Y);
                tappedView.InvokeDown();
            }
        }


        public void HandlePan(Point p)
        {
            if (_downTappable != null)
            {
                _dragPos = new Point(p.X + _dragPos.X, p.Y + _dragPos.Y);
                if (_downTappable.HitTheBox(_dragPos))
                {
                    _downTappable.InvokePan(p);
                }
                else
                {
                    _downTappable.InvokeDragOut();
                    _downTappable = null;
                }
            }
        }

        public void HandleSwipe(Point p, Direction direction)
        {
            _downTappable?.InvokeSwipe(direction);
        }

        public ISkiaView DetectInteractedTappable(IList<ISkiaView> tappables, Point p)
        {
            foreach (var tappable in tappables.Where(t => t.IsVisible).OrderByDescending(t => t.ZIndex).ToList())
            {
                if (tappable.HitTheBox(p))
                {
                    return tappable;
                }
            }

            return null;
        }
    }
}
