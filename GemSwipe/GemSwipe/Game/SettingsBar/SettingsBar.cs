using GemSwipe.Game.SettingsBar.SettingOptions;
using GemSwipe.Paladin.Containers;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GemSwipe.Game.SettingsBar
{
    public class SettingsBar : SkiaView
    {
        private IList<OptionButton> _listOfSettingsButton;
        private readonly int _numberOfButtons = 6;
        private SettingsBarButton _settingsBarButton;
        private OptionsContainer _optionsContainer;
        private bool _switchOn = false;

        public SettingsBar(float x, float y, float width, float height) : base(x, y, height, width)
        {
            Hide();
            float buttonX = width/2;
            float buttonMargin = Height/((_numberOfButtons + 1)*(_numberOfButtons + 1));
            float buttonRadius = (Height /(_numberOfButtons + 1))/2;
            float topMargin = buttonMargin;

            float initY = topMargin + buttonRadius;
            float incrementY = buttonRadius + buttonMargin + buttonRadius;

            _settingsBarButton = new SettingsBarButton(buttonX, initY, buttonRadius);
            AddChild(_settingsBarButton);
            _settingsBarButton.Activated += Switch;

            _listOfSettingsButton = new List<OptionButton>()
            {
                new GetLifeOptionButton(buttonX,initY+incrementY,buttonRadius),
                new OptionButton(buttonX,initY+2*incrementY,buttonRadius),
                new OptionButton(buttonX,initY+3*incrementY,buttonRadius),
                new RestartGameOptionButton(buttonX,initY+4*incrementY,buttonRadius),
                new ExitGameOptionButton(buttonX,initY+5*incrementY,buttonRadius),
            };

            _optionsContainer = new OptionsContainer(width, 0,0,0);
            AddChild(_optionsContainer);

            foreach (OptionButton optionButton in _listOfSettingsButton)
            {
                DeclareTappable(optionButton);
                _optionsContainer.AddChild(optionButton);
                optionButton.Activated += optionButton.OnActivated;
            }
        }

        protected override void Draw()
        {
            DrawHitbox();
        }

        public void SetDefaultConfig()
        {
            var buttonToHide = _listOfSettingsButton.FirstOrDefault(p => p.SettingName == "RestartGame");
            buttonToHide.Hide();
            var buttonToHide2 = _listOfSettingsButton.FirstOrDefault(p => p.SettingName == "ExitGame");
            buttonToHide2.Hide();
        }

        public void SetInGameConfig()
        {
            foreach (OptionButton optionButton in _listOfSettingsButton)
            {
                optionButton.Show();
            }
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }

        public void OnAppearing()
        {

        }

        public void Switch()
        {
            if (_switchOn)
            {
                Close();
            }
            else 
            {
                Expand();
            }
        }

        public void Expand()
        {
            _switchOn = true;
            _optionsContainer.MoveToX(0);
        }

        public void Close()
        {
            _switchOn = false;
            _optionsContainer.MoveToX(Width);
        }

    }
}
