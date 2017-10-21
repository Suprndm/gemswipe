using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Entities;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities.Buttons;

namespace GemSwipe.Game.Test
{
    public class GemTester:SkiaView
    {
        private Gem _gem;
        public GemTester(float x, float y, float height, float width) : base(x, y, height, width)
        {

            var newButton = new TextButton(width / 2, 7.5f * height / 10, height / 40, "New");
            var popButton = new TextButton(width / 2, 8.5f * height / 10, height / 40, "Pop");
            var dieButton = new TextButton(width / 2, 9.5f * height / 10, height / 40, "Die");

            AddChild(newButton);
            AddChild(popButton);
            AddChild(dieButton);

            newButton.Activated += NewButton_Activated;
            popButton.Activated += PopButton_Activated;
            dieButton.Activated += DieButton_Activated;
        }

        private void NewButton_Activated()
        {
            _gem = new Gem(0, 0, 1, Width / 2, Height / 2, Width / 10);
            AddChild(_gem);
        }

        private void DieButton_Activated()
        {
            _gem.DieTo(Width / 2, Height/5 );
        }

        private void PopButton_Activated()
        {
            _gem.Pop();
        }


        protected override void Draw()
        {
        }
    }
}
