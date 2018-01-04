using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Effects;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.UIElements;
using GemSwipe.Paladin.Utilities;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game
{
    public class ObjectiveView : SkiaView
    {
        private readonly TextBlock _countText;
        public ObjectiveView(KeyValuePair<int, int> objective, bool inGame, float x, float y, float height, float width) : base(x, y, height, width)
        {
            var gemRadius = height / 2 * 0.5f;
            var gem = new Gem(objective.Key, -gemRadius + width/2, -gemRadius+  0.25f*height, gemRadius, new Random());
            gem.Pop();
            AddChild(gem);

            var textHeight = height * 0.25f;
            if (inGame)
            {
                var text = new TextBlock(-textHeight*0.15f+ width/2, height*0.75f, $"/{objective.Value}", textHeight,
                    new SKColor(255, 255, 255), HorizontalAlignment.Right);
                AddChild(text);

                _countText = new TextBlock(-textHeight * 0.15f + width / 2, height * 0.75f, 0.ToString(), textHeight,
                    new SKColor(255, 255, 255),HorizontalAlignment.Left);
                AddChild(_countText);
            }
            else
            {
                var text = new TextBlock(0 + width / 2, height * 0.75f, objective.Value.ToString(), textHeight,
                    new SKColor(255, 255, 255));
                AddChild(text);
            }
        }

        protected override void Draw()
        {
        }

        public void UpdateCount(int count)
        {
            if (_countText != null)
                _countText.Text = count.ToString();
        }
    }
}
