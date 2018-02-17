using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Sprites;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GemSwipe.Paladin.UIElements
{
    public class SlidingCollectionIndicator : SkiaView
    {
        private Sprite _buttonSprite;

        private readonly float _size = SkiaRoot.ScreenWidth * 0.02f;
        private readonly float _selectedSize = SkiaRoot.ScreenWidth * 0.03f;
        private readonly float _initialOpacity = 0.5f;

        private const uint AnimationMs = 500;


        public SlidingCollectionIndicator()
        {
            Width = _size;
            Height = _size;
            _buttonSprite = new Sprite(SpriteConst.LevelBase, 0, 0, _size, _size, new SKPaint { Color = new SKColor(255, 255, 255) });
            AddChild(_buttonSprite);
        }

        public void Select()
        {
            this.Animate("indicatorWidth", p => _buttonSprite.Width = (float)p, _buttonSprite.Width, _selectedSize, 4, AnimationMs, Easing.CubicInOut);
            this.Animate("indicatorHeight", p => _buttonSprite.Height = (float)p, _buttonSprite.Height, _selectedSize, 4, AnimationMs, Easing.CubicInOut);
            this.Animate("indicatorOpacity", p => _opacity = (float)p, _opacity, 1, 4, AnimationMs, Easing.CubicInOut);
        }

        public void Unselect()
        {
            this.Animate("indicatorWidth", p => _buttonSprite.Width = (float)p, _buttonSprite.Width, _size, 4, AnimationMs, Easing.CubicInOut);
            this.Animate("indicatorHeight", p => _buttonSprite.Height = (float)p, _buttonSprite.Height, _size, 4, AnimationMs, Easing.CubicInOut);
            this.Animate("indicatorOpacity", p => _opacity = (float)p, _opacity, _initialOpacity, 4, AnimationMs, Easing.CubicInOut);
        }

        protected override void Draw()
        {
        }
    }
}
