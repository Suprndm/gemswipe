using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Effects;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Popups;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities.Buttons;

namespace GemSwipe.Game.Test
{
    public class PopupTester : SkiaView
    {
        public PopupTester(float height, float width) : base(0, 0, height, width)
        {
            var popButton = new TextButton(width / 2, 9.5f * height / 10, height / 40, "Pop !");
            AddChild(popButton);

            popButton.Activated += PopButton_Activated;
        }

        private void PopButton_Activated()
        {
            var popupDialogue = new DialogPopup(true);
            AddChild(popupDialogue);
            popupDialogue.Show();

            //var background = new PopupBackground(Width, Height);
            //AddChild(background);
            //var textblock = new TextBlock(0,0, "Your mission is :", Height/50, CreateColor(255,255,255), HorizontalAlignment.Right);
            //var popup = new Popup(textblock, Height, Width);
            //AddChild(popup);

            //background.Show();
            //popup.Show();

            //popup.NextAction += () =>
            //{
            //    background.Hide();
            //};

            //popup.BackAction += () =>
            //{
            //    background.Hide();
            //};

            //background.Activated += () =>
            //{
            //    popup.HideLeft();
            //    background.Hide();
            //};
        }


        protected override void Draw()
        {
        }
    }
}
