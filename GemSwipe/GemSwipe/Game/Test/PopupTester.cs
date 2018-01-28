using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Effects;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Popups;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.UIElements.Buttons;
using GemSwipe.Paladin.UIElements.Popups;

namespace GemSwipe.Game.Test
{
    public class PopupTester : SkiaView
    {
        public PopupTester()
        {
            var popButton = new TextButton(Width / 2, 9.5f * Height / 10, Height / 40, "Pop !");
            AddChild(popButton);

            popButton.Activated += PopButton_Activated;
        }

        private void PopButton_Activated()
        {
            var popupDialogue = new WinDialogPopup(5);
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
