using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Sprites;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Effects
{
    public class StarEffect : SkiaView
    {
        private Sprite _starBackFilledSprite;

        private Sprite _starFilledSprite;
        private float _starFilledSize;
        private float _starFilledOpacity;

        private Sprite _starStrokeSprite;
        private float _starStrokeSize;
        private float _starStrokeOpacity;
        public StarEffect(float x, float y) : base(x, y, SkiaRoot.ScreenHeight * 0.08f, SkiaRoot.ScreenHeight * 0.08f)
        {
            _starBackFilledSprite = new Sprite(SpriteConst.SmallStarBackFilled, 0, 0, Width, Width, new SKPaint { Color = new SKColor(100, 100, 100) });
            AddChild(_starBackFilledSprite);

            _starFilledSprite = new Sprite(SpriteConst.SmallStarFilled, 0, 0, Width, Width, new SKPaint { Color = new SKColor(255, 255, 255) });
            _starStrokeSprite = new Sprite(SpriteConst.SmallStarStroke, 0, 0, Width, Width, new SKPaint { Color = new SKColor(255, 255, 255) });


        }

        public async Task Start()
        {
            AddChild(_starFilledSprite);
            AddChild(_starStrokeSprite);

            Task.Run(() =>
            {
                this.Animate("starFilledOpacity", p => _starFilledOpacity = (float) p, 0, 1, 4, (uint) 400,
                    Easing.CubicOut);
                this.Animate("starFilledSize", p => _starFilledSize = (float) p, 0, Width, 4, (uint) 200,
                    Easing.CubicOut);
            });

            await Task.Delay(100);
            this.Animate("starStrokeOpacity", p => _starStrokeOpacity = (float)p, 0, 1, 4, (uint)400, Easing.CubicOut);
            this.Animate("starStrokeSize", p => _starStrokeSize = (float)p, Width*4, Width*1f, 4, (uint)400, Easing.CubicOut);
        }

        protected override void Draw()
        {
            _starFilledSprite.Opacity = _starFilledOpacity;
            _starFilledSprite.Width = _starFilledSize;
            _starFilledSprite.Height = _starFilledSize;

             _starStrokeSprite.Opacity = _starStrokeOpacity;
             _starStrokeSprite.Width = _starStrokeSize;
             _starStrokeSprite.Height = _starStrokeSize;
        }
    }
}
