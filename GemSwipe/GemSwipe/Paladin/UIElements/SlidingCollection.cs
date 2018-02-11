using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GemSwipe.Paladin.UIElements
{
    public class SlidingCollection : SkiaView
    {
        private readonly int _slideMs = 1000;
        private readonly float _marginRatio;
        private IList<ISkiaView> _items;
        private float? _lastPanX;
        protected SKRect _hitbox;
        private int _currentIndex;
        private float _initialX;
        private float _slideRatio = 0.3f;
        private float _totalSlide;

        private bool _isFullScreen;

        public SlidingCollection(
            float x,
            float y,
            float width,
            float height,
            IList<ISkiaView> items,
            float marginRatio = 0,
            int slideMs = 1000,
            float slideRatio = 0.3f,
            int initialIndex = 0,
            bool isFullScreen = true) : base(x, y, height, width)
        {
            _initialX = x;
            _items = items;
            _marginRatio = marginRatio;
            DeclareTappable(this);
            _currentIndex = initialIndex;
            _slideMs = slideMs;
            _slideRatio = slideRatio;
            _isFullScreen = isFullScreen;

            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                item.X = 0 + i * (Width + _marginRatio * SkiaRoot.ScreenWidth);
                item.Y = 0;

                AddChild(item);
            }

            _currentIndex = 0;

            if (_isFullScreen)
            {
                _hitbox = SKRect.Create(X, Y, Width, Height);
            }
            else
            {
                _hitbox = SKRect.Create(X - Width / 2, Y - Height / 2, Width, Height);
            }


            Up += () => Release();
            Pan += (p) => OnPan((float)p.X);
        }

        private void OnPan(float x)
        {
            _totalSlide += x;
            X += x;
            return;
        }

        public override SKRect GetHitbox()
        {
            return _hitbox;
        }

        private void Release()
        {
            if (_totalSlide < 0 && Math.Abs(_totalSlide) > Width * _slideRatio && _currentIndex < _items.Count - 1)
            {
                _currentIndex++;
            }
            else
            if (_totalSlide > 0 && _totalSlide > Width * _slideRatio && _currentIndex > 0)
            {
                _currentIndex--;
            }

            _lastPanX = null;

            var itemToSlideTo = _items[_currentIndex];
            var itemToSlideToRealX = itemToSlideTo.X - X;
            var targetX = -itemToSlideToRealX + _initialX;

            this.Animate("slide", p => _x = (float)p, _x, targetX, 4, (byte)_slideMs, Easing.CubicOut);
            _totalSlide = 0;
        }

        public void AddItem(ISkiaView item)
        {
            _items.Add(item);
        }

        public void RemoveItem(ISkiaView item)
        {
            _items.Remove(item);
        }

        protected override void Draw()
        {
        }
    }
}
