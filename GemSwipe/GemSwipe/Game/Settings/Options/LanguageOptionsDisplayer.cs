using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings.Options
{
    public class LanguageOptionsDisplayer : OptionsDisplayer
    {
        private int _animationMs = 600;
        private float _topMargin;
        private float _buttonMargin;
        private IList<string> _listOfLanguages;
        private IList<OptionPickerButton> _listOfLanguageButtons;

        public LanguageOptionsDisplayer(float x, float y, float width, float height) : base(x,y,width,height)
        {
            SettingName = SettingsEnum.Language;
            IsVisible = false;

            _listOfLanguageButtons = new List<OptionPickerButton>();

            _listOfLanguages = new List<string>()
            {
                "English",
                "Français",
                "Español",
                "Italiano",

            };

            _topMargin = height / 10;
            _buttonMargin = height / 10;

            float buttonY = _topMargin - _buttonMargin;

            foreach (string language in _listOfLanguages)
            {
                buttonY += _buttonMargin;
                OptionPickerButton languageButton = new OptionPickerButton(width/2, buttonY, height/40,language);
                AddChild(languageButton);
                _listOfLanguageButtons.Add(languageButton);
            }
        }

        public override Task Display()
        {
            IsVisible = true;
            this.Animate("LightButton", p => Opacity = (float)p, Opacity, 1, 8, (uint)_animationMs,
                Easing.Linear);
            //foreach (OptionPickerButton languageButton in _listOfLanguageButtons)
            //{
            //    languageButton.Animate("LightButton", p => languageButton.Opacity = (float)p, 0, 1, 8, (uint)_animationMs,
            //        Easing.Linear);
            //}
            return Task.Delay(_animationMs);
        }

        public override Task Hide()
        {
            this.Animate("FadeButton", p => Opacity = (float)p, Opacity, 0, 8, (uint)_animationMs,
                Easing.Linear);

            //foreach (OptionPickerButton languageButton in _listOfLanguageButtons)
            //{
            //    languageButton.Animate("FadeButton", p => languageButton.TextColor = new SKColor(255, 255, 255, (byte)p), 255, 0, 8, (uint)_animationMs,
            //        Easing.CubicInOut);
            //    return Task.Delay(_animationMs);
            //}
            return Task.Delay(_animationMs);
        }


        protected override void Draw()
        {
            base.Draw();
        }
    }
}
