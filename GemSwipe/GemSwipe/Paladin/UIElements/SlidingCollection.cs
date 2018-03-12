using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Containers;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace GemSwipe.Paladin.UIElements
{
    public class SlidingCollection<T> : SkiaView
        where T : ISkiaView
    {
        private readonly int _slideMs = 1000;
        private readonly float _marginRatio;
        private IList<T> _items;
        private float? _lastPanX;
        protected SKRect _hitbox;
        private int _currentIndex;
        private float _initialX;
        private float _slideRatio;
        private float _totalSlide;

        private bool _isFullScreen;

        public event Action<T> OnNext;

        private IList<SlidingCollectionIndicator> _indicators;
        private Container _itemsContainer;

        public SlidingCollection(
            float x,
            float y,
            float width,
            float height,
            IList<T> items,
            float marginRatio = 0,
            int slideMs = 1000,
            float slideRatio = 0.15f,
            int initialIndex = 0,
            bool isFullScreen = true) : base(x, y, height, width)
        {
            _initialX = x;
            _items = items;
            _marginRatio = marginRatio;
            DeclarePannable(this);
            _currentIndex = initialIndex;
            _slideMs = slideMs;
            _slideRatio = slideRatio;
            _isFullScreen = isFullScreen;

            _indicators = new List<SlidingCollectionIndicator>();
            _itemsContainer = new Container();
            AddChild(_itemsContainer);

            var indicatorSpace = SkiaRoot.ScreenWidth * 0.08f;
            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                item.X = 0 + i * (Width + _marginRatio * SkiaRoot.ScreenWidth);
                item.Y = 0;

                _itemsContainer.AddContent(item);
            }

            for (int i = 0; i < _items.Count; i++)
            {
                var indicator = new SlidingCollectionIndicator();
                indicator.X = i * indicatorSpace - (indicatorSpace * _items.Count) / 2 + Width / 2 + indicatorSpace / 2;
                indicator.Y = SkiaRoot.ScreenHeight * 0.97f;

                AddChild(indicator);
                _indicators.Add(indicator);

                if (i == 0)
                {
                    indicator.Select();
                }
                else
                {
                    indicator.Unselect();
                }
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
            DragOut += () => Release();
        }

        private void OnPan(float x)
        {
            _totalSlide += x;
            _itemsContainer.X += x;
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
                _indicators[_currentIndex].Unselect();
                _currentIndex++;
                _indicators[_currentIndex].Select();

                OnNext?.Invoke(_items[_currentIndex]);
            }
            else
            if (_totalSlide > 0 && _totalSlide > Width * _slideRatio && _currentIndex > 0)
            {
                _indicators[_currentIndex].Unselect();
                _currentIndex--;
                _indicators[_currentIndex].Select();

                OnNext?.Invoke(_items[_currentIndex]);
            }

            _lastPanX = null;

            var itemToSlideTo = _items[_currentIndex];
            var itemToSlideToRealX = itemToSlideTo.X - _itemsContainer.X;
            var targetX = -itemToSlideToRealX + _initialX;

            this.Animate("slide", p => _itemsContainer.X = (float)p, _itemsContainer.X, targetX, 4, (byte)_slideMs, Easing.CubicOut);
            _totalSlide = 0;
        }

        protected override void Draw()
        {
        }
    }
}
