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
        private ISkiaView _downPannable;
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
            var tappedView = DetectInteractedViews(tappables, p);
            if (tappedView != null && tappedView.IsEnabled)
            {
                tappedView.InvokeUp();
            }

            _downTappable = null;

            if(_downPannable != null)
            {
                _downPannable.InvokeUp();
                _downPannable = null;
            }
        }

        public void HandleDown(IList<ISkiaView> tappables, IList<ISkiaView> pannables, Point p)
        {
            _downPos = p;

            var tappedView = DetectInteractedViews(tappables, p);
            if (tappedView != null && tappedView.IsEnabled)
            {
                _downTappable = tappedView;
                _dragPos = new Point(p.X, p.Y);
                tappedView.InvokeDown();
            }

            tappedView = DetectInteractedViews(pannables, p);
            if (tappedView != null && tappedView.IsEnabled)
            {
                _downPannable = tappedView;
                _dragPos = new Point(p.X, p.Y);
                tappedView.InvokeDown();
            }
        }


        public void HandlePan(Point p)
        {
            if (_downPannable != null)
            {
                _dragPos = new Point(p.X + _dragPos.X, p.Y + _dragPos.Y);
                if (_downPannable.HitTheBox(_dragPos))
                {
                    _downPannable.InvokePan(p);
                }
                else
                {
                    _downPannable.InvokeDragOut();
                    _downPannable = null;
                }
            }
        }

        public void HandleSwipe(Point p, Direction direction)
        {
            _downPannable?.InvokeSwipe(direction);
        }

        public ISkiaView DetectInteractedViews(IList<ISkiaView> views, Point p)
        {
            foreach (var view in views.Where(t => t.IsVisible).OrderByDescending(t => t.ZIndex).ToList())
            {
                if (view.HitTheBox(p))
                {
                    return view;
                }
            }

            return null;
        }
    }
}
