using GemSwipe.Paladin.UIElements.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.SettingsBar.SettingOptions
{
    public class OptionButton : RoundButton
    {
        public OptionButton(float x, float y, float radius) : base(x, y, radius)
        {

        }

        public virtual void OnActivated()
        {
        }
       
        public void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }

    }
}
