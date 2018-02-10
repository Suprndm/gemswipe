using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Events;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Popups;
using GemSwipe.Game.Shards;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Behaviors;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Sprites;
using GemSwipe.Paladin.UIElements.Buttons;
using Newtonsoft.Json;
using SkiaSharp;

namespace GemSwipe.Game.Test
{
    class SpriteTester : SkiaView
    {
        private Sprite _sprite;
        private Sprite _sprite2;
        private Sprite _sprite3;
        private Sprite _sprite4;
        public SpriteTester()
        {
            var popButton = new TextButton(Width / 2, 9.5f * Height / 10, Height / 40, "Pop !");
            AddChild(popButton);

            popButton.Activated += PopButton_Activated;


        }

        private void PopButton_Activated()
        {
           var logoSprite = new Sprite(SpriteConst.BelowTheStars, Width / 2, Height * 0.2f, Width * 0.33f, Height * 0.2f, new SKPaint { Color = new SKColor(255, 255, 255), BlendMode = SKBlendMode.Plus });
            logoSprite.Opacity = 0.7f;
            logoSprite.AddBehavior(new ToastBehavior(0, 0.3f, 1, 500, 500));
            AddChild(logoSprite);

        }



        protected override void Draw()
        {
   
        }
    }
}
