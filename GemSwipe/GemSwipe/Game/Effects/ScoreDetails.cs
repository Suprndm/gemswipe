using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.UIElements;
using GemSwipe.Paladin.VisualEffects;

namespace GemSwipe.Game.Effects
{
    public class ScoreDetails:SkiaView
    {
        private IList<TextBlock> _textBlocks;

        public ScoreDetails(float x, float y, float width, float height) : base(x, y, height, width)
        {
            _textBlocks = new List<TextBlock>();
        }

        public async Task Push(string text)
        {
            var textblock = CreateTextblock(text);
            AddChild(textblock);

            textblock.Show();
        }

        private ToastText CreateTextblock(string text)
        {
            var textblock = new ToastText(0, _textBlocks.Count * Height / 4-Height/1.3f, text, Height/7, CreateColor(255,255,255));
            _textBlocks.Add(textblock);
            return textblock;
        }

        protected override void Draw()
        {
            
        }
    }
}
