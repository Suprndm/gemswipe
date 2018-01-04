using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.UIElements.Buttons;

namespace GemSwipe.Game.Test
{
    public class GemTester:SkiaView
    {
        private Gem _gem;
        public GemTester()
        {

            var levelUpButton = new TextButton(Width / 2, 6.5f * Height / 10, Height / 40, "Level up");
            var newButton = new TextButton(Width / 2, 7.5f * Height / 10, Height / 40, "New");
            var popButton = new TextButton(Width / 2, 8.5f * Height / 10, Height / 40, "Pop");
            var dieButton = new TextButton(Width / 2, 9.5f * Height / 10, Height / 40, "Die");

            AddChild(levelUpButton);
            AddChild(newButton);
            AddChild(popButton);
            AddChild(dieButton);

            levelUpButton.Activated += LevelUpButton_Activated; ;
            newButton.Activated += NewButton_Activated;
            popButton.Activated += PopButton_Activated;
            dieButton.Activated += DieButton_Activated;
        }

        private void LevelUpButton_Activated()
        {
            _gem.LevelUp();
            _gem.Resolve();
            _gem.Fuse();
        }

        private void NewButton_Activated()
        {
            _gem = new Gem( 1, Width / 2 - Width/10, Height /3, Width / 10, new Random());
            AddChild(_gem);
        }

        private void DieButton_Activated()
        {
            _gem.DieTo(Width / 2 - Width / 10, Height/5 );
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
