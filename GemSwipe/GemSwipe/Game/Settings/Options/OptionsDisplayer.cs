using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings.Options
{
    public class OptionsDisplayer
    {
        private SettingOptionButton _languageButton;
        private SettingButton _settingButton;
        public OptionsDisplayer(SettingButton settingButton)
        {
            _languageButton = new SettingOptionButton(settingButton.X, settingButton.Y, 200, settingButton.Text);
            _settingButton = settingButton;
            DisplayOptions();
        }

        private Task DisplayOptions()
        {
            return Task.Delay(0);
        }
    }
}
